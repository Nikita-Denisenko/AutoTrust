using AutoTrust.Application.Common;
using AutoTrust.Application.Interfaces.Validators;
using AutoTrust.Application.Models.DTOs.Requests.CreateDtos;

namespace AutoTrust.Application.Validators
{
    public class NotificationValidator : INotificationValidator
    {
        private readonly IUserValidator _userValidator;

        public NotificationValidator(IUserValidator userValidator)
        {
            _userValidator = userValidator;
        }

        public async Task<ValidationResult> CanCreateAsync(CreateNotificationDto dto, CancellationToken cancellationToken)
        {
            var (userExists, error) = await _userValidator.IsUserExistsAsync(dto.UserId, cancellationToken);
            return new ValidationResult(userExists, userExists ? null : error);
        }
    }
}
