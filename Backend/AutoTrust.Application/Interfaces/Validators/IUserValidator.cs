using AutoTrust.Application.Common;
using AutoTrust.Application.Models.DTOs.AuthDtos;
using AutoTrust.Application.Models.DTOs.Requests.UpdateDtos.User;

namespace AutoTrust.Application.Interfaces.Validators
{
    public interface IUserValidator
    {
        public Task<ValidationResult> IsUserExistsAsync(int id, CancellationToken cancellationToken);
        public Task<ValidationResult> CanCreateAsync(RegisterDto dto, CancellationToken cancellationToken);
        public Task<ValidationResult> CanUpdateAsync(UpdateUserInfoDto dto, CancellationToken cancellationToken);
    }
}
