using AutoTrust.Application.Models.DTOs.Requests.CreateDtos;
using AutoTrust.Application.Models.DTOs.Requests.FilterDtos.Reaction;
using AutoTrust.Application.Models.DTOs.Responses.CreatedDtos;
using AutoTrust.Application.Models.DTOs.Responses.ReadDtos.ReactionDtos;

namespace AutoTrust.Application.Interfaces.Services
{
    public interface IReactionService
    {
        public Task<CreatedReactionDto> CreatedReactionAsync(int currentUserId, CreateReactionDto dto, CancellationToken cancellationToken);
        public Task<List<ReactionDto>> GetReactionsAsync(int listingId, ReactionFilterDto filterDto, CancellationToken cancellationToken);
        public Task<List<AdminListingReactionDto>> GetListingReactionsAsync(int listingId, AdminReactionFilterDto filterDto, CancellationToken cancellationToken);
        public Task<List<AdminUserReactionDto>> GetUserReactionsAsync(int userId, AdminReactionFilterDto filterDto, CancellationToken cancellationToken);
        public Task DeleteReactionAsync(int id, int currentUserId, CancellationToken cancellationToken);
    }
}
