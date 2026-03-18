using AutoTrust.Application.Interfaces.Validators;
using AutoTrust.Application.Models.DTOs.Requests.CreateDtos;
using AutoTrust.Application.Models.DTOs.Requests.UpdateDtos.Car;
using AutoTrust.Application.Common;
using AutoTrust.Domain.Entities;
using AutoTrust.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;


namespace AutoTrust.Application.Validators
{
    public class CarValidator : ICarValidator
    {
        private readonly IRepository<Car> _repo;

        public CarValidator(IRepository<Car> repo) => _repo = repo;

        private async Task<bool> IsStateNumberUniqueForCreate(string stateNumber, CancellationToken cancellationToken)
        {
            return !await _repo.GetQuery()
                .AnyAsync(c => c.StateNumber.Value == stateNumber, cancellationToken);  
        }

        private async Task<bool> IsStateNumberUniqueForUpdate(int id, string stateNumber, CancellationToken cancellationToken)
        {
            return !await _repo.GetQuery()
                .AnyAsync(c => c.Id != id && c.StateNumber.Value == stateNumber, cancellationToken);
        }

        public async Task<ValidationResult> CanCreate(CreateCarDto dto, CancellationToken cancellationToken)
        {
            var result = await IsStateNumberUniqueForCreate(dto.StateNumber, cancellationToken);
            return new ValidationResult(result, result ? null : "Car with this state number already exists!");
        }

        public async Task<ValidationResult> CanUpdate(int id, UpdateCarDto dto, CancellationToken cancellationToken)
        {
            var stateNumber = dto.StateNumber;
            var result = stateNumber == null || await IsStateNumberUniqueForUpdate(id, stateNumber, cancellationToken);
            return new ValidationResult(result, result ? null : "Car with this state number already exists!");
        }
    }
}
