using AutoTrust.Application.Models.DTOs.Requests.CreateDtos;
using AutoTrust.Application.Models.DTOs.Requests.FilterDtos.Follow;
using AutoTrust.Application.Models.DTOs.Responses.CreatedDtos;
using AutoTrust.Application.Models.DTOs.Responses.ReadDtos.FollowDtos;

namespace AutoTrust.Application.Interfaces.Services
{
    public interface IFollowService
    {
        public Task<CreatedFollowDto> CreateFollowAsync(int currentUserId, CreateFollowDto dto, CancellationToken cancellationToken);
        public Task<List<UserFollowDto>> GetUserFollowsAsync(int currentUserId, FollowFilterDto filterDto, CancellationToken cancellationToken);
        public Task<List<UserFollowerDto>> GetUserFollowersAsync(int currentUserId, FollowFilterDto filterDto, CancellationToken cancellationToken);
        public Task DeleteFollowAsync(int id, int currentUserId, CancellationToken cancellationToken);
    }
}
