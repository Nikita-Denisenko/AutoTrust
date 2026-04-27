using AutoMapper;
using AutoMapper.QueryableExtensions;
using AutoTrust.Application.Interfaces.Repositories;
using AutoTrust.Application.Interfaces.Services;
using AutoTrust.Application.Interfaces.Validators;
using AutoTrust.Application.Models.DTOs.Requests.Actions.Car;
using AutoTrust.Application.Models.DTOs.Requests.CreateDtos;
using AutoTrust.Application.Models.DTOs.Requests.FilterDtos.Car;
using AutoTrust.Application.Models.DTOs.Requests.UpdateDtos.Car;
using AutoTrust.Application.Models.DTOs.Responses.CreatedDtos;
using AutoTrust.Application.Models.DTOs.Responses.ReadDtos.CarDtos;
using AutoTrust.Domain.Entities;
using AutoTrust.Domain.Enums.OrderParams;
using AutoTrust.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace AutoTrust.Application.Services
{
    public class CarService : ICarService
    {
        private readonly IRepository<Car> _repo;
        private readonly ICarValidator _validator;
        private readonly IUserValidator _userValidator;
        private readonly IMapper _mapper;

        public CarService
        (
            IRepository<Car> repo,
            ICarValidator validator,
            IUserValidator userValidator,
            IMapper mapper
        )
        {
            _repo = repo;
            _validator = validator;
            _userValidator = userValidator;
            _mapper = mapper;
        }

        private IQueryable<Car> ApplyFilters(CarFilterDto filterDto, int? userId = null)
        {
            var query = _repo.GetQuery()
                .AsNoTracking();

            if (filterDto is AdminCarFilterDto adminFilterDto)
            {
                if (adminFilterDto.IsDeleted != null)
                    query = query.Where(c => c.IsDeleted == adminFilterDto.IsDeleted.Value);
            }
            else
                query = query.Where(c => !c.IsDeleted);

            if (filterDto is AdminCarFilterDto adminDto)
            {
                if (adminDto.UserId != null)
                {
                    if (adminDto.OnlyCurrent)
                        query = query.Where(c => c.OwnershipHistory.OrderBy(co => co.FromDate).Last().UserId == adminDto.UserId);
                    else
                        query = query.Where(c => c.OwnershipHistory.Any(oh => oh.UserId == adminDto.UserId));
                }
            }
            else
                query = query.Where(c => c.OwnershipHistory.OrderBy(co => co.FromDate).Last().UserId == userId);

            if (filterDto.Color != null)
                query = query.Where(c => c.Color == filterDto.Color);

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

        public async Task<CreatedCarDto> CreateCarAsync(int currentUserId, CreateCarDto createCarDto, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.CanCreateAsync(createCarDto, cancellationToken);

            if (!validationResult.IsValid)
                throw new InvalidOperationException(validationResult.ErrorMessage);

            try
            {
                var car = _mapper.Map<Car>(createCarDto, opts => opts.Items["UserId"] = currentUserId);

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

        public async Task DeleteCarAsync(int id, int currentUserId, CancellationToken cancellationToken)
        {
            var car = await _repo.GetByIdAsync(id, cancellationToken);

            if (car == null)
                throw new KeyNotFoundException($"Car by Id {id} was not found!");

            var currentOwnership = car.OwnershipHistory.FirstOrDefault(o => o.IsCurrent);
            if (currentOwnership == null || currentOwnership.UserId != currentUserId)
                throw new InvalidOperationException("You can only delete your own cars");

            try
            {
                car.Delete();
                await _repo.SaveChangesAsync(cancellationToken);
            }
            catch (InvalidOperationException ex)
            {
                throw new InvalidOperationException($"Failed to delete Car: {ex.Message}", ex);
            }
        }

        public async Task<PublicCarDto> GetCarAsync(int id, CancellationToken cancellationToken)
        {
            var car = await _repo.GetQuery()
                 .AsNoTracking()
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
                .AsNoTracking()
                .Where(c => c.Id == id)
                .ProjectTo<AdminCarDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(cancellationToken);

            if (car == null)
                throw new KeyNotFoundException($"Car by Id {id} was not found!");

            return car;
        }

        public Task<List<PublicCarListItemDto>> GetCarsAsync(CarFilterDto filterDto, int userId, CancellationToken cancellationToken)
        {
            var query = ApplyFilters(filterDto, userId);

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

        public async Task UpdateCarAsync(int id, int currentUserId, UpdateCarDto updateCarDto, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.CanUpdateAsync(id, updateCarDto, cancellationToken);

            if (!validationResult.IsValid)
                throw new InvalidOperationException(validationResult.ErrorMessage);

            var car = await _repo.GetByIdAsync(id, cancellationToken)
                ?? throw new KeyNotFoundException($"Car by Id {id} was not found!");

            var currentOwnership = car.OwnershipHistory.FirstOrDefault(o => o.IsCurrent);
            if (currentOwnership == null || currentOwnership.UserId != currentUserId)
                throw new InvalidOperationException("You can only update your own cars");

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

        public async Task MakeForSaleAsync(int id, int currentUserId, CancellationToken cancellationToken)
        {
            var car = await _repo.GetByIdAsync(id, cancellationToken)
                ?? throw new KeyNotFoundException($"Car by Id {id} was not found!");

            var currentOwnership = car.OwnershipHistory.FirstOrDefault(o => o.IsCurrent);
            if (currentOwnership == null || currentOwnership.UserId != currentUserId)
                throw new InvalidOperationException("You can only put your own cars for sale");

            try
            {
                car.MakeForSale();
                await _repo.SaveChangesAsync(cancellationToken);
            }
            catch (InvalidOperationException ex)
            {
                throw new InvalidOperationException($"Failed to make Car for sale: {ex.Message}", ex);
            }
        }

        public async Task TakeOffSaleAsync(int id, int currentUserId, CancellationToken cancellationToken)
        {
            var car = await _repo.GetByIdAsync(id, cancellationToken)
                ?? throw new KeyNotFoundException($"Car by Id {id} was not found!");

            var currentOwnership = car.OwnershipHistory.FirstOrDefault(o => o.IsCurrent);
            if (currentOwnership == null || currentOwnership.UserId != currentUserId)
                throw new InvalidOperationException("You can only take off your own cars from sale");

            try
            {
                car.TakeOffSale();
                await _repo.SaveChangesAsync(cancellationToken);
            }
            catch (InvalidOperationException ex)
            {
                throw new InvalidOperationException($"Failed to remove Car from sale: {ex.Message}", ex);
            }
        }

        public async Task TransferOwnershipAsync(int id, int currentUserId, TransferOwnershipDto transferOwnershipDto, CancellationToken cancellationToken)
        {
            var car = await _repo.GetQuery()
                .Include(c => c.OwnershipHistory)
                .FirstOrDefaultAsync(c => c.Id == id, cancellationToken)
                ?? throw new KeyNotFoundException($"Car by Id {id} was not found!");

            var currentOwnership = car.OwnershipHistory.FirstOrDefault(o => o.IsCurrent);
            if (currentOwnership == null || currentOwnership.UserId != currentUserId)
                throw new InvalidOperationException("You can only transfer ownership of your own cars");

            var (newOwnerExists, error) = await _userValidator.IsUserExistsAsync(transferOwnershipDto.NewOwnerId, cancellationToken);

            if (!newOwnerExists)
                throw new InvalidOperationException($"Failed to transfer Car ownership: {error}");

            try
            {
                car.TransferOwnership(transferOwnershipDto.NewOwnerId, Url.Create(transferOwnershipDto.BillOfSalePhotoUrl));
                await _repo.SaveChangesAsync(cancellationToken);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                throw new InvalidOperationException($"Failed to transfer Car ownership: {ex.Message}", ex);
            }
            catch (InvalidOperationException ex)
            {
                throw new InvalidOperationException($"Failed to transfer Car ownership: {ex.Message}", ex);
            }
        }
    }
}