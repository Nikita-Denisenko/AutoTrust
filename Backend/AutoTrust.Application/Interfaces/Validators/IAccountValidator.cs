using AutoTrust.Application.Common;
using AutoTrust.Application.Models.DTOs.AuthDtos;

namespace AutoTrust.Application.Interfaces.Validators
{
    public interface IAccountValidator
    {
        public Task<ValidationResult> CanCreate(RegisterDto registerDto, CancellationToken cancellationToken);
        public Task<ValidationResult> CanChangePhone(int id, string newPhone, CancellationToken cancellationToken);
    }
}
