using AutoTrust.Application.Models.DTOs.Requests.CreateDtos;
using AutoTrust.Application.Models.DTOs.Requests.FilterDtos.Comment;
using AutoTrust.Application.Models.DTOs.Requests.UpdateDtos.Comment;
using AutoTrust.Application.Models.DTOs.Responses.CreatedDtos;
using AutoTrust.Application.Models.DTOs.Responses.ReadDtos.CommentDtos;

namespace AutoTrust.Application.Interfaces.Services
{
    public interface ICommentService
    {
        public Task<CreatedCommentDto> CreateCommentAsync(int currentUserId, CreateCommentDto dto, CancellationToken cancellationToken);
        public Task<List<CommentDto>> GetCommentsAsync(int listingId, CommentFilterDto filterDto, CancellationToken cancellationToken);
        public Task<List<AdminCommentDto>> GetCommentsForAdminAsync(AdminCommentFilterDto adminFilterDto, CancellationToken cancellationToken);
        public Task UpdateCommentAsync(int id, int currentUserId, UpdateCommentDto dto, CancellationToken cancellationToken);
        public Task DeleteCommentAsync(int id, int currentUserId, CancellationToken cancellationToken);
        public Task BlockCommentByAdminAsync(int id, CancellationToken cancellationToken);
        public Task UnblockCommentByAdminAsync(int id, CancellationToken cancellationToken);
    }
}
