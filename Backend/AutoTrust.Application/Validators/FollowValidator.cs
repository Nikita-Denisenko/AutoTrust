using AutoTrust.Application.Common;
using AutoTrust.Application.Interfaces.Validators;
using AutoTrust.Application.Models.DTOs.Requests.CreateDtos;
using AutoTrust.Domain.Entities;
using AutoTrust.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AutoTrust.Application.Validators
{
    public class FollowValidator : IFollowValidator
    {
        private readonly IUserValidator _userValidator;

        public FollowValidator(IUserValidator userValidator, IRepository<User> userRepo) => _userValidator = userValidator;

        public async Task<ValidationResult> CanCreateAsync(int currentUserId, CreateFollowDto dto, CancellationToken cancellationToken)
        {
            if (currentUserId == dto.TargetId)
                return new ValidationResult(false, "You cannot follow yourself.");

            var (result, error) = await _userValidator.IsUserExistsAsync(dto.TargetId, cancellationToken);
            return new ValidationResult(result, error);
        }
    }
}
