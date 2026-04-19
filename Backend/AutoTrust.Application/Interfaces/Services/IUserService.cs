using AutoTrust.Application.Models.DTOs.Requests.FilterDtos.User;
using AutoTrust.Application.Models.DTOs.Requests.UpdateDtos.User;
using AutoTrust.Application.Models.DTOs.Responses.ReadDtos.UserDtos;

namespace AutoTrust.Application.Interfaces.Services
{
    public interface IUserService
    {
        public Task<UserProfileDto> GetUserByIdAsync(int id, CancellationToken cancellationToken);
        public Task<AdminUserDto> GetUserByIdForAdminAsync(int id, CancellationToken cancellationToken);
        public Task<List<UserProfileListItemDto>> GetUsersAsync(UserFilterDto filterDto, CancellationToken cancellationToken);
        public Task<List<AdminUserListItemDto>> GetUsersForAdminAsync(AdminUserFilterDto filterDto, CancellationToken cancellationToken);
        public Task UpdateUserAvatarAsync(int currentUserId, UpdateAvatarUrlDto dto, CancellationToken cancellationToken);
        public Task UpdateUserInfoAsync(int currentUserId, UpdateUserInfoDto dto, CancellationToken cancellationToken);
        public Task DeleteUserProfileAsync(int id, CancellationToken cancellationToken);
        public Task BlockUserAsync(int id, CancellationToken cancellationToken);
        public Task UnblockUserAsync(int id, CancellationToken cancellationToken);
    }
}
