using AutoTrust.Application.Interfaces;
using AutoTrust.Application.Models.DTOs.Requests.FilterDtos.Account;
using AutoTrust.Application.Models.DTOs.Requests.UpdateDtos.Account;
using AutoTrust.Application.Models.DTOs.Responses.ReadDtos.AccountDtos;
using AutoTrust.Domain.Entities;
using AutoTrust.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using static AutoTrust.Domain.Enums.OrderParams.AdminAccountOrderParam;

namespace AutoTrust.Application.Services
{
    public class AccountService : IAccountService
    {
        private readonly IRepository<Account> _repo;

        public AccountService(IRepository<Account> repo) => _repo = repo;

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
            var query = _repo.GetQuery();

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

        public async Task ChangeEmailAsync(ChangeEmailDto changeEmailDto, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task ChangePasswordAsync(ChangePasswordDto changePasswordDto, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task CgangePhoneAsync(ChangePhoneDto changePhoneDto, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
