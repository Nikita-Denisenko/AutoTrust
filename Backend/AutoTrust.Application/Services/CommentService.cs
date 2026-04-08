using AutoTrust.Application.Interfaces.Services;
using AutoTrust.Application.Models.DTOs.Requests.CreateDtos;
using AutoTrust.Application.Models.DTOs.Requests.FilterDtos.Comment;
using AutoTrust.Application.Models.DTOs.Requests.UpdateDtos.Comment;
using AutoTrust.Application.Models.DTOs.Responses.CreatedDtos;
using AutoTrust.Application.Models.DTOs.Responses.ReadDtos.CommentDtos;

namespace AutoTrust.Application.Services
{
    public class CommentService : ICommentService
    {
        public Task BlockCommentByAdminAsync(int Id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<CreatedCommentDto> CreateCommentAsync(int currentUserId, CreateCommentDto dto, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task DeleteCommentAsync(int Id, int currentUserId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<List<CommentDto>> GetCommentsAsync(int listingId, CommentFilterDto filterDto, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<List<AdminCommentDto>> GetCommentsForAdminAsync(AdminCommentFilterDto adminFilterDto, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task UnblockCommentByAdminAsync(int Id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task UpdateCommentAsync(int Id, int currentUserId, UpdateCommentDto dto, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
