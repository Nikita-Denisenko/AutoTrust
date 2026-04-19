using AutoTrust.Application.Models.DTOs.Requests.CreateDtos;
using AutoTrust.Application.Models.DTOs.Requests.FilterDtos.Review;
using AutoTrust.Application.Models.DTOs.Requests.UpdateDtos.Review;
using AutoTrust.Application.Models.DTOs.Responses.CreatedDtos;
using AutoTrust.Application.Models.DTOs.Responses.ReadDtos.ReviewDtos;

namespace AutoTrust.Application.Interfaces.Services
{
    public interface IReviewService
    {
        public Task<CreatedReviewDto> CreateReviewAsync(int currentUserId, CreateReviewDto dto, CancellationToken cancellationToken);
        public Task<List<ReviewDto>> GetReviewsAsync(int userId, ReviewFilterDto filterDto, CancellationToken cancellationToken);
        public Task<List<AdminReviewByUserDto>> GetReviewsByUserForAdminAsync(int userId, AdminReviewFilterDto filterDto, CancellationToken cancellationToken);
        public Task<List<AdminReviewToUserDto>> GetReviewsToUserForAdminAsync(int userId, AdminReviewFilterDto filterDto, CancellationToken cancellationToken);
        public Task UpdateReviewAsync(int id, int currentUserId, UpdateReviewDto dto, CancellationToken cancellationToken);
        public Task DeleteReviewAsync(int id, int currentUserId, CancellationToken cancellationToken);
    }
}
