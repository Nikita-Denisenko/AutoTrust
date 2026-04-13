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
        private readonly IRepository<Follow> _repo;

        public FollowValidator(IUserValidator userValidator, IRepository<Follow> repo) 
        {
            _userValidator = userValidator;
            _repo = repo;
        }
        public async Task<ValidationResult> CanCreateAsync(int currentUserId, CreateFollowDto dto, CancellationToken cancellationToken)
        {
            if (currentUserId == dto.TargetId)
                return new ValidationResult(false, "You cannot follow yourself.");

            bool isFollowExists = await _repo.GetQuery()
                .AsNoTracking()
                .AnyAsync(f => f.FollowerId == currentUserId && f.TargetId == dto.TargetId, cancellationToken);

            if (isFollowExists)
                return new ValidationResult(false, $"Follow to user with ID {dto.TargetId} is already exists!");

            var (result, error) = await _userValidator.IsUserExistsAsync(dto.TargetId, cancellationToken);
            return new ValidationResult(result, error);
        }
    }
}
