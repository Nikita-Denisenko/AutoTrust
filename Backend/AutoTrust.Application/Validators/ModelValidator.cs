using AutoTrust.Application.Common;
using AutoTrust.Application.Interfaces.Validators;
using AutoTrust.Application.Models.DTOs.Requests.CreateDtos;
using AutoTrust.Domain.Entities;
using AutoTrust.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AutoTrust.Application.Validators
{
    public class ModelValidator : IModelValidator
    {
        private readonly IRepository<Model> _modelRepo;
        private readonly IRepository<Brand> _brandRepo;

        public ModelValidator(IRepository<Model> modelRepo, IRepository<Brand> brandRepo)
        {
            _modelRepo = modelRepo;
            _brandRepo = brandRepo;
        }

        private async Task<bool> IsNameUniqueAsync(string name, CancellationToken cancellationToken)
        {
            return !await _modelRepo
                .GetQuery()
                .AnyAsync(m => m.Name == name, cancellationToken);
        }

        public async Task<ValidationResult> CanCreateAsync(CreateModelDto dto, CancellationToken cancellationToken)
        {
            bool brandExists = await _brandRepo
                .GetQuery()
                .AnyAsync(b => b.Id == dto.BrandId, cancellationToken);

            if (!brandExists)
                return new ValidationResult(false, $"Brand with ID {dto.BrandId} does not exists!");

            if (!await IsNameUniqueAsync(dto.Name, cancellationToken))
                return new ValidationResult(false, $"Model with name {dto.Name} already exists!");
           
            return new ValidationResult(true);
        }
    }
}
