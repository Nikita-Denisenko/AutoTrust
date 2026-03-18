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

        public BrandValidator(IRepository<Brand> repo) => _repo = repo;

        private async Task<bool> IsBrandNameUnique(string name, CancellationToken cancellationToken)
            => !await _repo.GetQuery()
            .AnyAsync(b => b.Name.ToLower() == name.ToLower(), cancellationToken);

        private async Task<bool> IsBrandNameUniqueForUpdate(int id, string name, CancellationToken cancellationToken)
            => !await _repo.GetQuery()
            .AnyAsync(b => b.Name.ToLower() == name.ToLower() && b.Id != id, cancellationToken);

        public async Task<ValidationResult> CanCreate(CreateBrandDto dto, CancellationToken cancellationToken)
        {
            var result = await IsBrandNameUnique(dto.Name, cancellationToken);
            return new ValidationResult(result, result ? null : "Brand with this name already exists.");
        }

        public async Task<ValidationResult> CanUpdate(int id, UpdateBrandDto dto, CancellationToken cancellationToken)
        {
            var name = dto.Name;
            var result = name == null || await IsBrandNameUniqueForUpdate(id, name, cancellationToken);
            return new ValidationResult(result, result ? null : "Brand with this name already exists.");
        }
    }
}
