using AutoTrust.Application.Models.DTOs.Requests.FilterDtos.Account;
using AutoTrust.Application.Models.DTOs.Requests.UpdateDtos.Account;
using AutoTrust.Application.Models.DTOs.Responses.ReadDtos.AccountDtos;

namespace AutoTrust.Application.Interfaces.Services
{
    public interface IAccountService
    {
        public Task<List<AdminAccountListItemDto>> GetAccountsForAdminAsync(AdminAccountFilterDto filterDto, CancellationToken cancellationToken);
        public Task<AccountDto> GetUserAccountAsync(int userId, CancellationToken cancellationToken);
        public Task<AdminAccountDto> GetAccountForAdminAsync(int id, CancellationToken cancellationToken);
        public Task ChangeEmailAsync(ChangeEmailDto changeEmailDto, CancellationToken cancellationToken);
        public Task ChangePasswordAsync(ChangePasswordDto changePasswordDto, CancellationToken cancellationToken);
        public Task CgangePhoneAsync(ChangePhoneDto changePhoneDto, CancellationToken cancellationToken);
    }
}