using AutoTrust.Application.Interfaces.Validators;
using AutoTrust.Application.Models.DTOs.Requests.CreateDtos;
using AutoTrust.Application.Models.DTOs.Requests.UpdateDtos.Car;
using AutoTrust.Application.Common;
using AutoTrust.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using AutoTrust.Application.Interfaces.Repositories;


namespace AutoTrust.Application.Validators
{
    public class CarValidator : ICarValidator
    {
        private readonly IRepository<Car> _repo;
        private readonly ILocationValidator _locationValidator;

        public CarValidator(IRepository<Car> repo, ILocationValidator locationValidator) 
        {
            _repo = repo;
            _locationValidator = locationValidator;
        }

        private async Task<bool> IsStateNumberUniqueForCreateAsync(string stateNumber, CancellationToken cancellationToken)
        {
            return !await _repo
                .GetQuery()
                .AsNoTracking()
                .AnyAsync(c => c.StateNumber.Value == stateNumber, cancellationToken);  
        }

        private async Task<bool> IsStateNumberUniqueForUpdateAsync(int id, string stateNumber, CancellationToken cancellationToken)
        {
            return !await _repo
                .GetQuery()
                .AsNoTracking()
                .AnyAsync(c => c.Id != id && c.StateNumber.Value == stateNumber, cancellationToken);
        }

        public async Task<ValidationResult> CanCreateAsync(CreateCarDto dto, CancellationToken cancellationToken)
        {
            var result = await IsStateNumberUniqueForCreateAsync(dto.StateNumber, cancellationToken);
            return new ValidationResult(result, result ? null : "Car with this state number already exists!");
        }

        public async Task<ValidationResult> CanUpdateAsync(int id, UpdateCarDto dto, CancellationToken cancellationToken)
        {
            var stateNumber = dto.StateNumber;
            var result = stateNumber == null || await IsStateNumberUniqueForUpdateAsync(id, stateNumber, cancellationToken);
            return new ValidationResult(result, result ? null : "Car with this state number already exists!");
        }
    }
}
