using AutoTrust.Application.Common;
using AutoTrust.Application.Models.DTOs.AuthDtos;

namespace AutoTrust.Application.Interfaces.Validators
{
    public interface IAccountValidator
    {
        public Task<ValidationResult> CanCreateAsync(RegisterDto registerDto, CancellationToken cancellationToken);
        public Task<ValidationResult> CanChangePhoneAsync(int id, string newPhone, CancellationToken cancellationToken);
    }
}
