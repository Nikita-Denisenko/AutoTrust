using AutoTrust.Application.Common;
using AutoTrust.Application.Interfaces.Repositories;
using AutoTrust.Application.Interfaces.Validators;
using AutoTrust.Application.Models.DTOs.AuthDtos;
using AutoTrust.Application.Models.DTOs.Requests.UpdateDtos.User;
using AutoTrust.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AutoTrust.Application.Validators
{
    public class UserValidator : IUserValidator
    {
        private readonly ILocationValidator _locationValidator;
        private readonly IRepository<User> _repo;

        public UserValidator(ILocationValidator locationValidator, IRepository<User> repo)
        {
            _locationValidator = locationValidator;
            _repo = repo;
        }

        public async Task<ValidationResult> CanCreateAsync(RegisterDto dto, CancellationToken cancellationToken)
        {
            var (isValid, errorMessage) = await _locationValidator.CityExistsAsync(dto.CityId, cancellationToken);
            return new ValidationResult(isValid, errorMessage);
        }

        public async Task<ValidationResult> CanUpdateAsync(UpdateUserInfoDto dto, CancellationToken cancellationToken)
        {
            if (dto.CityId == null)
                return new ValidationResult(true);

            var (isValid, errorMessage) = await _locationValidator.CityExistsAsync(dto.CityId.Value, cancellationToken);
            return new ValidationResult(isValid, errorMessage);
        }

        public async Task<ValidationResult> IsUserExistsAsync(int id, CancellationToken cancellationToken)
        {
            bool exists = await _repo.GetQuery().AnyAsync(u => u.Id == id, cancellationToken);
            return new ValidationResult(exists, exists ? null : $"User with id {id} does not exist.");
        }
    }
}
