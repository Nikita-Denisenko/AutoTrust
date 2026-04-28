using AutoTrust.Application.Interfaces.Services;
using AutoTrust.Application.Models.DTOs.Requests.Actions.Account;
using AutoTrust.Application.Models.DTOs.Requests.FilterDtos.Account;
using AutoTrust.Application.Models.DTOs.Responses.ReadDtos.AccountDtos;
using AutoTrust.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using static AutoTrust.Domain.Enums.OrderParams.AdminAccountOrderParam;
using AutoTrust.Application.Interfaces.Repositories;
using AutoTrust.Domain.ValueObjects;

namespace AutoTrust.Application.Services
{
    public class AccountService : IAccountService
    {
        private readonly IRepository<Account> _repo;
        private readonly IPasswordHasher _passwordHasher;

        public AccountService(IRepository<Account> repo, IPasswordHasher passwordHasher)
        {
            _repo = repo;
            _passwordHasher = passwordHasher;
        }

        public async Task<AccountDto> GetUserAccountAsync(int userId, CancellationToken cancellationToken)
        {
            var account = await _repo.GetQuery()
                .FirstOrDefaultAsync(a => a.UserId == userId, cancellationToken);

            if (account == null)
                throw new KeyNotFoundException($"Account by userId {userId} was not found!");

            return new AccountDto
            (
                account.Email.Value,
                account.Phone.Value,
                account.CreatedAt
            );
        }

        public async Task<AdminAccountDto> GetAccountForAdminAsync(int id, CancellationToken cancellationToken)
        {
            var account = await _repo.GetQuery()
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.Id == id, cancellationToken);

            if (account == null)
                throw new KeyNotFoundException($"Account by Id {id} was not found!");

            return new AdminAccountDto
            (
                id,
                account.UserId,
                account.Email.Value,
                account.Phone.Value,
                account.CreatedAt,
                account.IsDeleted
            );
        }

        public async Task<List<AdminAccountListItemDto>> GetAccountsForAdminAsync(AdminAccountFilterDto filterDto, CancellationToken cancellationToken)
        {
            var query = _repo.GetQuery().AsNoTracking();

            if (filterDto.IsDeleted != null)
                query = filterDto.IsDeleted.Value 
                    ? query.Where(a => a.IsDeleted) 
                    : query.Where(a => a.IsDeleted == false);

            query = query
                .Where(a => a.Email.Value.Contains(filterDto.SearchText));

            query = filterDto.OrderParam switch
            {
                CreatedDateTime => filterDto.ByAscending
                    ? query.OrderBy(a => a.CreatedAt)
                    : query.OrderByDescending(a => a.CreatedAt),
                
                _ => filterDto.ByAscending
                    ? query.OrderBy(a => a.Email)
                    : query.OrderByDescending(a => a.Email)
            };

            query = query
                .Skip((filterDto.Page - 1) * filterDto.Size)
                .Take(filterDto.Size);

            return await query.Select(a => new AdminAccountListItemDto
                (
                    a.Id,
                    a.UserId,
                    a.Email.Value
                )
            ).ToListAsync(cancellationToken);
        }

        public async Task ChangeEmailAsync(int userId, ChangeEmailDto changeEmailDto, CancellationToken cancellationToken)
        {
            var account = await _repo.GetQuery().FirstOrDefaultAsync(a => a.UserId == userId, cancellationToken);

            if (account == null)
                throw new KeyNotFoundException($"You cannot change other users email!");

            try
            {
                var email = new Email(changeEmailDto.Email);
                account.ChangeEmail(email);
                await _repo.SaveChangesAsync(cancellationToken);
            }
            catch (ArgumentException ex)
            {
                throw new InvalidOperationException($"Failed to update email: {ex.Message}");
            }
        }

        public async Task ChangePasswordAsync(int userId, ChangePasswordDto changePasswordDto, CancellationToken cancellationToken)
        {
            var account = await _repo.GetQuery().FirstOrDefaultAsync(a => a.UserId == userId, cancellationToken);

            if (account == null)
                throw new KeyNotFoundException($"You cannot change other users password!");

            if (!_passwordHasher.VerifyPassword(changePasswordDto.OldPassword, account.PasswordHash))
                throw new InvalidOperationException("Old Password was not verified!");

            var password = changePasswordDto.NewPassword;

            if (password.Length < 8)
                throw new InvalidOperationException($"Password Lenth cannot be smaller than 8 symbols.");
            
            var passwordHash = _passwordHasher.HashPassword(password);

            account.ChangePassword(passwordHash); 
            await _repo.SaveChangesAsync(cancellationToken);
        }

        public async Task ChangePhoneAsync(int userId, ChangePhoneDto changePhoneDto, CancellationToken cancellationToken)
        {
            var account = await _repo.GetQuery()
                .FirstOrDefaultAsync(a => a.UserId == userId, cancellationToken);

            if (account == null)
                throw new KeyNotFoundException($"You cannot change other users Phone Number!");

            try
            {
                var phone = new Phone(changePhoneDto.Phone);
                account.ChangePhone(phone);
                await _repo.SaveChangesAsync(cancellationToken);
            }
            catch (ArgumentException ex)
            {
                throw new InvalidOperationException($"Failed to update phone number: {ex.Message}");
            }
        }
    }
}
