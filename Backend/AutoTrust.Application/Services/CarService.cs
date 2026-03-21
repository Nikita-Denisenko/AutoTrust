using AutoMapper;
using AutoMapper.QueryableExtensions;
using AutoTrust.Application.Interfaces.Services;
using AutoTrust.Application.Interfaces.Validators;
using AutoTrust.Application.Models.DTOs.Requests.CreateDtos;
using AutoTrust.Application.Models.DTOs.Requests.FilterDtos.Brand;
using AutoTrust.Application.Models.DTOs.Requests.FilterDtos.Car;
using AutoTrust.Application.Models.DTOs.Requests.UpdateDtos.Car;
using AutoTrust.Application.Models.DTOs.Responses.CreatedDtos;
using AutoTrust.Application.Models.DTOs.Responses.ReadDtos.BrandDtos;
using AutoTrust.Application.Models.DTOs.Responses.ReadDtos.CarDtos;
using AutoTrust.Application.Models.DTOs.Responses.ReadDtos.LocationDTOs;
using AutoTrust.Application.Models.DTOs.Responses.ReadDtos.LocationDTOs.CityDtos;
using AutoTrust.Application.Models.DTOs.Responses.ReadDtos.LocationDTOs.CountryDtos;
using AutoTrust.Application.Models.DTOs.Responses.ReadDtos.ModelDtos;
using AutoTrust.Domain.Entities;
using AutoTrust.Domain.Enums.OrderParams;
using AutoTrust.Domain.Interfaces;
using AutoTrust.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace AutoTrust.Application.Services
{
    public class CarService : ICarService
    {
        private readonly IRepository<Car> _repo;
        private readonly ICarValidator _validator;
        private readonly IMapper _mapper;

        public CarService
        (
            IRepository<Car> repo, 
            ICarValidator validator, 
            IMapper mapper
        )
        {
            _repo = repo;
            _validator = validator;
            _mapper = mapper;
        }

        private IQueryable<Car> ApplyFilters(CarFilterDto filterDto)
        {
            var query = _repo.GetQuery();

            if (filterDto is AdminCarFilterDto adminFilterDto && adminFilterDto.IsDeleted != null)
                query = query.Where(c => c.IsDeleted == adminFilterDto.IsDeleted.Value);
            else
                query = query.Where(c => !c.IsDeleted);

            query = query.Where
            (
                c => (c.Model.Name + c.Model.Brand.Name).ToLower()
                     .Contains(filterDto.SearchText.ToLower())
            );

            if (filterDto.InSale != null)
                query = query.Where(c => c.InSale == filterDto.InSale);

            if (filterDto.HasAccident != null)
                query = query.Where(c => c.HasAccident == filterDto.HasAccident);

            if (filterDto.HadMajorRepair != null)
                query = query.Where(c => c.OwnershipHistory.Any(co => co.HadMajorRepair) == filterDto.HadMajorRepair);

            query = filterDto.OrderParam switch
            {
                CarOrderParam.EngineMileage => filterDto.ByAscending
                    ? query.OrderBy(c => c.EngineMileage)
                    : query.OrderByDescending(c => c.EngineMileage),

                CarOrderParam.OwnershipsQuantity => filterDto.ByAscending
                    ? query.OrderBy(c => c.OwnershipHistory.Count)
                    : query.OrderByDescending(c => c.OwnershipHistory.Count),

                _ => filterDto.ByAscending
                    ? query.OrderBy(c => c.ReleaseYear)
                    : query.OrderByDescending(c => c.ReleaseYear),
            };

            query = query.Skip((filterDto.Page - 1) * filterDto.Size)
               .Take(filterDto.Size);

            return query;
        }

        public async Task<CreatedCarDto> CreateCarAsync(CreateCarDto createCarDto, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.CanCreate(createCarDto, cancellationToken);

            if (!validationResult.IsValid)
                throw new InvalidOperationException(validationResult.ErrorMessage);

            try
            {
                var car = _mapper.Map<Car>(createCarDto);

                await _repo.AddAsync(car, cancellationToken);
                await _repo.SaveChangesAsync(cancellationToken);

                return _mapper.Map<CreatedCarDto>(car);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                throw new InvalidOperationException($"Failed to create Car: {ex.Message}", ex);
            }
            catch (ArgumentException ex) 
            {
                throw new InvalidOperationException($"Failed to create Car: {ex.Message}", ex);
            }
        }

        public async Task DeleteCarAsync(int id, CancellationToken cancellationToken)
        {
            var car = await _repo.GetByIdAsync(id, cancellationToken);

            if (car == null)
                throw new KeyNotFoundException($"Car by Id {id} was not found!");

            car.Delete();

            await _repo.SaveChangesAsync(cancellationToken);
        }

        public async Task<PublicCarDto> GetCarAsync(int id, CancellationToken cancellationToken)
        {
           var car = await _repo.GetQuery()
                .Where(c => c.Id == id)
                .ProjectTo<PublicCarDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(cancellationToken);
               
           if (car == null)
               throw new KeyNotFoundException($"Car by Id {id} was not found!");

            return car;
        }

        public async Task<AdminCarDto> GetCarForAdminAsync(int id, CancellationToken cancellationToken)
        {
            var car = await _repo.GetQuery()
                .Where(c => c.Id == id)
                .ProjectTo<AdminCarDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(cancellationToken);

            if (car == null)
                throw new KeyNotFoundException($"Car by Id {id} was not found!");

            return car;
        }

        public Task<List<PublicCarListItemDto>> GetCarsAsync(CarFilterDto filterDto, CancellationToken cancellationToken)
        {
            var query = ApplyFilters(filterDto);

            return _mapper
                .ProjectTo<PublicCarListItemDto>(query)
                .ToListAsync(cancellationToken);
        }

        public Task<List<AdminCarListItemDto>> GetCarsForAdminAsync(AdminCarFilterDto filterDto, CancellationToken cancellationToken)
        {
            var query = ApplyFilters(filterDto);

            return _mapper
                .ProjectTo<AdminCarListItemDto>(query)
                .ToListAsync(cancellationToken);
        }

        public async Task UpdateCarAsync(int id, UpdateCarDto updateCarDto, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.CanUpdate(id, updateCarDto, cancellationToken);

            if (!validationResult.IsValid)
                throw new InvalidOperationException(validationResult.ErrorMessage);

            var car = await _repo.GetByIdAsync(id, cancellationToken) 
                ?? throw new KeyNotFoundException($"Car by Id {id} was not found!");

            try
            {
                car.UpdateInfo
                (
                    updateCarDto.Description,
                    updateCarDto.ImageUrl != null ? Url.Create(updateCarDto.ImageUrl) : null,
                    updateCarDto.Color,
                    updateCarDto.StateNumber != null ? new StateNumber(updateCarDto.StateNumber) : null,
                    updateCarDto.EngineMileage,
                    updateCarDto.HasAccident
                );

                await _repo.SaveChangesAsync(cancellationToken);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                throw new InvalidOperationException($"Failed to update Car: {ex.Message}", ex);
            }
            catch (ArgumentException ex)
            {
                throw new InvalidOperationException($"Failed to update Car: {ex.Message}", ex);
            }
        }
    }
}
