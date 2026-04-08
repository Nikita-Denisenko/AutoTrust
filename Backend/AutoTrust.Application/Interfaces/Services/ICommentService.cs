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
        public Task UpdateCommentAsync(int Id, int currentUserId, UpdateCommentDto dto, CancellationToken cancellationToken);
        public Task DeleteCommentAsync(int Id, int currentUserId, CancellationToken cancellationToken);
        public Task BlockCommentByAdminAsync(int Id, CancellationToken cancellationToken);
        public Task UnblockCommentByAdminAsync(int Id, CancellationToken cancellationToken);
    }
}
