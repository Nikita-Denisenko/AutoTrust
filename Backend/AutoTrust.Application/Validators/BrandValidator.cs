using AutoTrust.Application.Common;
using AutoTrust.Application.Interfaces.Validators;
using AutoTrust.Application.Models.DTOs.Requests.CreateDtos;
using AutoTrust.Application.Models.DTOs.Requests.UpdateDtos.Brand;
using AutoTrust.Domain.Entities;
using AutoTrust.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AutoTrust.Application.Validators
{
    public class BrandValidator : IBrandValidator
    {
        private readonly IRepository<Brand> _repo;
        private readonly ILocationValidator _locationValidator;

        public BrandValidator(IRepository<Brand> repo, ILocationValidator locationValidator)
        {
            _repo = repo;
            _locationValidator = locationValidator;
        }

        private async Task<bool> IsBrandNameUniqueAsync(string name, CancellationToken cancellationToken)
            => !await _repo
                .GetQuery()
                .AsNoTracking()
                .AnyAsync(b => b.Name.ToLower() == name.ToLower(), cancellationToken);

        private async Task<bool> IsBrandNameUniqueForUpdateAsync(int id, string name, CancellationToken cancellationToken)
            => !await _repo
                .GetQuery()
                .AsNoTracking()
                .AnyAsync(b => b.Name.ToLower() == name.ToLower() && b.Id != id, cancellationToken);

        public async Task<ValidationResult> CanCreateAsync(CreateBrandDto dto, CancellationToken cancellationToken)
        {
            var (isValid, error) = await _locationValidator.CountryExistsAsync(dto.CountryId, cancellationToken);

            if (!isValid) 
                return new ValidationResult(false, error);

            var result = await IsBrandNameUniqueAsync(dto.Name, cancellationToken);
            return new ValidationResult(result, result ? null : "Brand with this name already exists.");
        }

        public async Task<ValidationResult> CanUpdateAsync(int id, UpdateBrandDto dto, CancellationToken cancellationToken)
        {
            var name = dto.Name;
            var result = name == null || await IsBrandNameUniqueForUpdateAsync(id, name, cancellationToken);
            return new ValidationResult(result, result ? null : "Brand with this name already exists.");
        }
    }
}
