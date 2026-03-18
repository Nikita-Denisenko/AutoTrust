using AutoTrust.Application.Interfaces.Services;
using AutoTrust.Application.Interfaces.Validators;
using AutoTrust.Application.Models.DTOs.Requests.CreateDtos;
using AutoTrust.Application.Models.DTOs.Requests.UpdateDtos.Car;
using AutoTrust.Application.Models.DTOs.Responses.CreatedDtos;
using AutoTrust.Application.Models.DTOs.Responses.ReadDtos.BrandDtos;
using AutoTrust.Application.Models.DTOs.Responses.ReadDtos.CarDtos;
using AutoTrust.Application.Models.DTOs.Responses.ReadDtos.LocationDTOs;
using AutoTrust.Application.Models.DTOs.Responses.ReadDtos.LocationDTOs.CityDtos;
using AutoTrust.Application.Models.DTOs.Responses.ReadDtos.LocationDTOs.CountryDtos;
using AutoTrust.Application.Models.DTOs.Responses.ReadDtos.ModelDtos;
using AutoTrust.Domain.Entities;
using AutoTrust.Domain.Interfaces;
using AutoTrust.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace AutoTrust.Application.Services
{
    public class CarService : ICarService
    {
        private readonly IRepository<Car> _repo;
        private readonly ICarValidator _validator;

        public CarService(IRepository<Car> repo, ICarValidator validator)
        {
            _repo = repo;
            _validator = validator;
        }

        public async Task<CreatedCarDto> CreateCarAsync(CreateCarDto createCarDto, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.CanCreate(createCarDto, cancellationToken);

            if (!validationResult.IsValid)
                throw new InvalidOperationException(validationResult.ErrorMessage);

            try
            {
                var car = new Car
                (
                    createCarDto.Description,
                    createCarDto.ReleaseYear,
                    Url.Create(createCarDto.ImageUrl),
                    createCarDto.Color,
                    new StateNumber(createCarDto.StateNumber),
                    createCarDto.EngineMileage,
                    createCarDto.ModelId,
                    createCarDto.LocationCityId,
                    createCarDto.LocationCountryId,
                    createCarDto.HasAccident
                );

                await _repo.AddAsync(car, cancellationToken);
                await _repo.SaveChangesAsync(cancellationToken);

                return new CreatedCarDto
                (
                    car.Id,
                    car.ModelId,
                    car.LocationCityId,
                    car.LocationCountryId
                );
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
                .Include(c => c.Model)
                .ThenInclude(m => m.Brand)
                .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

           if (car == null)
               throw new KeyNotFoundException($"Car by Id {id} was not found!");

            return new PublicCarDto
            (
                car.Id,
                car.Description,
                car.ReleaseYear,
                car.ImageUrl.Value,
                car.Color,
                car.StateNumber.Value,
                car.EngineMileage,
                car.OwnershipHistory.Count,
                new ModelShortDto
                (
                    car.Model.Id,
                    car.Model.Name,
                    new BrandShortDto
                    (
                        car.Model.Brand.Id,
                        car.Model.Brand.Name,
                        car.Model.Brand.LogoUrl.ToString()
                    )
                ),
                new LocationDto
                (
                    new CityDto
                    (
                        car.LocationCity.Id,
                        car.LocationCity.Name
                    ),
                    new CountryShortDto
                    (
                        car.LocationCountryId,
                        car.LocationCountry.RuName,
                        car.LocationCountry.FlagImageUrl.ToString()
                    )
                ),
                car.HasAccident,
                car.InSale
            );
        }

        public async Task<AdminCarDto> GetCarForAdminAsync(int id, CancellationToken cancellationToken)
        {
            var car = await _repo.GetQuery()
                .Include(c => c.Model)
                .ThenInclude(m => m.Brand)
                .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

            if (car == null)
                throw new KeyNotFoundException($"Car by Id {id} was not found!");

            return new AdminCarDto
            (
                car.Id,
                car.Description,
                car.ReleaseYear,
                car.ImageUrl.Value,
                car.Color,
                car.StateNumber.Value,
                car.EngineMileage,
                car.OwnershipHistory.Count,
                new ModelShortDto
                (
                    car.Model.Id,
                    car.Model.Name,
                    new BrandShortDto
                    (
                        car.Model.Brand.Id,
                        car.Model.Brand.Name,
                        car.Model.Brand.LogoUrl.ToString()
                    )
                ),
                new LocationDto
                (
                    new CityDto
                    (
                        car.LocationCity.Id,
                        car.LocationCity.Name
                    ),
                    new CountryShortDto
                    (
                        car.LocationCountryId,
                        car.LocationCountry.RuName,
                        car.LocationCountry.FlagImageUrl.ToString()
                    )
                ),
                car.HasAccident,
                car.InSale,
                car.IsDeleted
            );
        }

        public Task<List<PublicCarListItemDto>> GetCarsAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<List<AdminCarListItemDto>> GetCarsForAdminAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
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
