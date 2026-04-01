using AutoTrust.Application.Common;
using AutoTrust.Application.Interfaces.Validators;
using AutoTrust.Application.Models.DTOs.AuthDtos;
using AutoTrust.Domain.Entities;
using AutoTrust.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AutoTrust.Application.Validators
{
    public class AccountValidator : IAccountValidator
    {
        private IRepository<Account> _repo;

        public AccountValidator(IRepository<Account> repo) =>  _repo = repo;

        private async Task<bool> IsEmailUniqueAsync(string email, CancellationToken cancellationToken)
            => !await _repo
                .GetQuery()
                .AsNoTracking()
                .AnyAsync(a => a.Email.Value == email, cancellationToken);

        private async Task<bool> IsPhoneUniqueAsync(string phone, CancellationToken cancellationToken)
            => !await _repo
                .GetQuery()
                .AsNoTracking()
                .AnyAsync(a => a.Phone.Value == phone, cancellationToken);

        private async Task<bool> IsPhoneUniqueForUpdateAsync(int id, string phone, CancellationToken cancellationToken)
            => !await _repo
                .GetQuery()
                .AsNoTracking()
                .AnyAsync(a => a.Phone.Value == phone && a.Id != id, cancellationToken);

        public async Task<ValidationResult> CanCreateAsync(RegisterDto registerDto, CancellationToken cancellationToken)
        {
            if(!await IsEmailUniqueAsync(registerDto.Email, cancellationToken))
                return new ValidationResult(false, "Account with this email already exists!");

            if(!await IsPhoneUniqueAsync(registerDto.Phone, cancellationToken))
                return new ValidationResult(false, "Account with this phone already exists!");

            return new ValidationResult(true);
        }

        public async Task<ValidationResult> CanChangePhoneAsync(int id, string newPhone, CancellationToken cancellationToken)
        {
            if(!await IsPhoneUniqueForUpdateAsync(id, newPhone, cancellationToken))
                return new ValidationResult(false, "Account with this phone already exists!");

            if (await _repo.GetQuery()
                .AsNoTracking()
                .AnyAsync(a => a.Phone.Value == newPhone && a.Id == id, cancellationToken))
                 return new ValidationResult(false, "you can't change your phone to the same one.");

            return new ValidationResult(true);
        }
    }
}
