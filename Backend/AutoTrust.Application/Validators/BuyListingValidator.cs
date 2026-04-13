using AutoTrust.Application.Common;
using AutoTrust.Application.Interfaces.Validators;
using AutoTrust.Application.Models.DTOs.Requests.CreateDtos;
using AutoTrust.Application.Models.DTOs.Requests.UpdateDtos.Listing;
using AutoTrust.Domain.Entities;
using AutoTrust.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;


namespace AutoTrust.Application.Validators
{
    public class BuyListingValidator : IBuyListingValidator
    {
        private readonly IRepository<Model> _modelRepo;
        private readonly ILocationValidator _locationValidator;

        public BuyListingValidator
        (
            IRepository<Model> modelRepo, 
            ILocationValidator locationValidator)
        {
            _modelRepo = modelRepo;
            _locationValidator = locationValidator;
        }

        public async Task<ValidationResult> CanCreateAsync(CreateBuyListingDto dto, CancellationToken cancellationToken)
        {
            var (cityExists, error) = await _locationValidator.CityExistsAsync(dto.CityId, cancellationToken);

            if (!cityExists)
                return new ValidationResult(false, error);

            bool modelExists = await _modelRepo.GetQuery()
                .AsNoTracking()
                .AnyAsync(m => m.Id == dto.ModelId, cancellationToken);

            return new ValidationResult(modelExists, modelExists ? null : $"Model with ID {dto.ModelId} does not exists!");
        }

        public async Task<ValidationResult> CanUpdateAsync(UpdateBuyListingDto dto, CancellationToken cancellationToken)
        {
            if (dto.ModelId == null)
                return new ValidationResult(true);

            bool modelExists = await _modelRepo.GetQuery()
               .AsNoTracking()
               .AnyAsync(m => m.Id == dto.ModelId, cancellationToken);

            return new ValidationResult(modelExists, modelExists ? null : $"Model with ID {dto.ModelId} does not exists!");
        }
    }
}