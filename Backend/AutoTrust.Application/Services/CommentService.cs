using AutoMapper;
using AutoTrust.Application.Interfaces.Repositories;
using AutoTrust.Application.Interfaces.Services;
using AutoTrust.Application.Interfaces.Validators;
using AutoTrust.Application.Models.DTOs.Requests.CreateDtos;
using AutoTrust.Application.Models.DTOs.Requests.FilterDtos.Comment;
using AutoTrust.Application.Models.DTOs.Requests.UpdateDtos.Comment;
using AutoTrust.Application.Models.DTOs.Responses.CreatedDtos;
using AutoTrust.Application.Models.DTOs.Responses.ReadDtos.CommentDtos;
using AutoTrust.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AutoTrust.Application.Services
{
    public class CommentService : ICommentService
    {
        private readonly IRepository<Comment> _repo;
        private readonly IUserValidator _userValidator;
        private readonly IRepository<Listing> _listingRepo;
        private readonly ICommentValidator _validator;
        private readonly IMapper _mapper;

        public CommentService
        (
            IRepository<Comment> repo,
            IUserValidator userValidator,
            IRepository<Listing> listingRepo,
            IMapper mapper,
            ICommentValidator validator
        )
        {
            _repo = repo;
            _userValidator = userValidator;
            _listingRepo = listingRepo;
            _mapper = mapper;
            _validator = validator;
        }

        private IQueryable<Comment> ApplyFilters(IQueryable<Comment> query, CommentFilterDto filterDto, int? listingId = null)
        {
            if (filterDto is AdminCommentFilterDto adminFilterDto)
            {
                if (adminFilterDto.IsDeleted != null)
                    query = query.Where(c => c.IsDeleted == adminFilterDto.IsDeleted);

                if (adminFilterDto.UserId != null)
                    query = query.Where(c => c.UserId == adminFilterDto.UserId);

                if (adminFilterDto.ListingId != null)
                    query = query.Where(c => c.ListingId == adminFilterDto.ListingId);
            }
            else
            {
                query = query.Where(c => !c.IsDeleted);

                if (listingId != null)
                    query = query.Where(c => c.ListingId == listingId);
            }

            query = filterDto.SortByAsc 
                ? query.OrderBy(c => c.CreatedAt)
                : query.OrderByDescending(c => c.CreatedAt);

            return query
                .Skip((filterDto.Page - 1) * filterDto.Size)
                .Take(filterDto.Size);
        }

        public async Task BlockCommentByAdminAsync(int id, CancellationToken cancellationToken)
        {
            var comment = await _repo.GetByIdAsync(id, cancellationToken);

            if (comment == null)
                throw new KeyNotFoundException($"Comment by Id {id} was not found to block!");

            comment.Block();

            await _repo.SaveChangesAsync(cancellationToken);
        }

        public async Task<CreatedCommentDto> CreateCommentAsync(int currentUserId, CreateCommentDto dto, CancellationToken cancellationToken)
        {
            var (canCreate, error) = await _validator.CanCreateAsync(dto, cancellationToken);

            if (!canCreate)
                throw new InvalidOperationException($"Failed to create Comment: {error}");

            try
            {
                var comment = _mapper.Map<Comment>(dto, opts => opts.Items["UserId"] = currentUserId);

                await _repo.AddAsync(comment, cancellationToken);
                await _repo.SaveChangesAsync(cancellationToken);

                return _mapper.Map<CreatedCommentDto>(comment);
            }
            catch(ArgumentOutOfRangeException ex)
            {
                throw new InvalidOperationException($"Failed to create Comment: {ex}", ex);
            }
            catch (ArgumentException ex)
            {
                throw new InvalidOperationException($"Failed to create Comment: {ex}", ex);
            }
        }

        public async Task DeleteCommentAsync(int id, int currentUserId, CancellationToken cancellationToken)
        {
            var comment = await _repo.GetByIdAsync(id, cancellationToken);

            if (comment == null)
                throw new KeyNotFoundException($"Comment by Id {id} was not found to delete!");

            if (comment.UserId != currentUserId)
                throw new InvalidOperationException($"User cannot delete other users's comments!");

            comment.Delete();

            await _repo.SaveChangesAsync(cancellationToken);
        }

        public async Task<List<CommentDto>> GetCommentsAsync(int listingId, CommentFilterDto filterDto, CancellationToken cancellationToken)
        {
            var listingExists = await _listingRepo
                .GetQuery()
                .AnyAsync(c => c.Id == listingId, cancellationToken);

            if (!listingExists)
                throw new KeyNotFoundException($"Listing with ID {listingId} does not exists!");

            var query = _repo.GetQuery().AsNoTracking();
            var comments = ApplyFilters(query, filterDto, listingId);
            return await _mapper.ProjectTo<CommentDto>(comments).ToListAsync(cancellationToken);
        }

        public async Task<List<AdminCommentDto>> GetCommentsForAdminAsync(AdminCommentFilterDto adminFilterDto, CancellationToken cancellationToken)
        {
            if (adminFilterDto.ListingId != null)
            {
                var listingExists = await _listingRepo
               .GetQuery()
               .AnyAsync(u => u.Id == adminFilterDto.ListingId, cancellationToken);

                if (!listingExists)
                    throw new KeyNotFoundException($"Listing with ID {adminFilterDto.ListingId} does not exists!");
            }

            if (adminFilterDto.UserId != null)
            {
                var (userExists, error) = await _userValidator.IsUserExistsAsync(adminFilterDto.UserId.Value, cancellationToken);

                if (!userExists)
                    throw new KeyNotFoundException(error);
            }

            var query = _repo.GetQuery().AsNoTracking();
            var comments = ApplyFilters(query, adminFilterDto);
            return await _mapper.ProjectTo<AdminCommentDto>(comments).ToListAsync(cancellationToken);
        }

        public async Task UnblockCommentByAdminAsync(int id, CancellationToken cancellationToken)
        {
            var comment = await _repo.GetByIdAsync(id, cancellationToken);

            if (comment == null)
                throw new KeyNotFoundException($"Comment by Id {id} was not found to unblock!");

            comment.Unblock();

            await _repo.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateCommentAsync(int id, int currentUserId, UpdateCommentDto dto, CancellationToken cancellationToken)
        {
            var comment = await _repo.GetByIdAsync(id, cancellationToken);

            if (comment == null)
                throw new KeyNotFoundException($"Comment by Id {id} was not found to update!");

            if (comment.UserId != currentUserId)
                throw new InvalidOperationException($"User cannot update other users's comments!");

            try
            {
                comment.UpdateText(dto.Text);
                await _repo.SaveChangesAsync(cancellationToken);
            }
            catch (ArgumentException ex)
            {
                throw new InvalidOperationException($"Failed to update Comment: {ex.Message}", ex);
            }
        }
    }
}
