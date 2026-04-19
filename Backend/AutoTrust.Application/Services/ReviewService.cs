using AutoMapper;
using AutoMapper.QueryableExtensions;
using AutoTrust.Application.Interfaces.Repositories;
using AutoTrust.Application.Interfaces.Services;
using AutoTrust.Application.Interfaces.Validators;
using AutoTrust.Application.Models.DTOs.Requests.CreateDtos;
using AutoTrust.Application.Models.DTOs.Requests.FilterDtos.Review;
using AutoTrust.Application.Models.DTOs.Requests.UpdateDtos.Review;
using AutoTrust.Application.Models.DTOs.Responses.CreatedDtos;
using AutoTrust.Application.Models.DTOs.Responses.ReadDtos.ReviewDtos;
using AutoTrust.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AutoTrust.Application.Services
{
    public class ReviewService : IReviewService
    {
        private readonly IRepository<Review> _repo;
        private readonly IReviewValidator _validator;
        private readonly IMapper _mapper;

        public ReviewService(
            IRepository<Review> repo,
            IReviewValidator validator,
            IMapper mapper)
        {
            _repo = repo;
            _validator = validator;
            _mapper = mapper;
        }

        private IQueryable<Review> ApplyFilters(
            ReviewFilterDto filterDto,
            int? userId = null,
            bool isAdmin = false,
            bool byUser = true)
        {
            var query = _repo.GetQuery().AsNoTracking();

            if (filterDto is AdminReviewFilterDto adminDto && adminDto.IsDeleted != null)
                query = query.Where(r => r.IsDeleted == adminDto.IsDeleted.Value);
            else if (!isAdmin)
                query = query.Where(r => !r.IsDeleted);

            if (userId.HasValue)
            {
                if (byUser)
                    query = query.Where(r => r.ReviewerId == userId.Value);
                else
                    query = query.Where(r => r.ReceiverId == userId.Value);
            }

            if (filterDto.Stars.HasValue)
                query = query.Where(r => r.Stars == filterDto.Stars.Value);

            query = filterDto.SortByAsc
                ? query.OrderBy(r => r.CreatedAt)
                : query.OrderByDescending(r => r.CreatedAt);

            query = query
                .Skip((filterDto.Page - 1) * filterDto.Size)
                .Take(filterDto.Size);

            return query;
        }

        public async Task<CreatedReviewDto> CreateReviewAsync(
            int currentUserId,
            CreateReviewDto dto,
            CancellationToken cancellationToken)
        {
            var validationResult = await _validator.CanCreateAsync(currentUserId, dto, cancellationToken);

            if (!validationResult.IsValid)
                throw new InvalidOperationException($"Failed to create review: {validationResult.ErrorMessage}");

            try
            {
                var review = _mapper.Map<Review>(dto, opts => opts.Items["ReviewerId"] = currentUserId);
                await _repo.AddAsync(review, cancellationToken);
                await _repo.SaveChangesAsync(cancellationToken);
                return _mapper.Map<CreatedReviewDto>(review);
            }
            catch (ArgumentException ex)
            {
                throw new InvalidOperationException($"Failed to create review: {ex.Message}", ex);
            }
        }

        public async Task<List<ReviewDto>> GetReviewsAsync(
            int userId,
            ReviewFilterDto filterDto,
            CancellationToken cancellationToken)
        {
            var query = ApplyFilters(filterDto, userId, isAdmin: false, byUser: false);
            return await _mapper
                .ProjectTo<ReviewDto>(query)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<AdminReviewByUserDto>> GetReviewsByUserForAdminAsync(
            int userId,
            AdminReviewFilterDto filterDto,
            CancellationToken cancellationToken)
        {
            var query = ApplyFilters(filterDto, userId, isAdmin: true, byUser: true);
            return await _mapper
                .ProjectTo<AdminReviewByUserDto>(query)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<AdminReviewToUserDto>> GetReviewsToUserForAdminAsync(
            int userId,
            AdminReviewFilterDto filterDto,
            CancellationToken cancellationToken)
        {
            var query = ApplyFilters(filterDto, userId, isAdmin: true, byUser: false);
            return await _mapper
                .ProjectTo<AdminReviewToUserDto>(query)
                .ToListAsync(cancellationToken);
        }

        public async Task UpdateReviewAsync(
            int id,
            int currentUserId,
            UpdateReviewDto dto,
            CancellationToken cancellationToken)
        {
            var review = await _repo.GetByIdAsync(id, cancellationToken);

            if (review == null)
                throw new KeyNotFoundException($"Review with ID {id} not found");

            if (review.ReviewerId != currentUserId)
                throw new UnauthorizedAccessException("You can only update your own reviews");

            try
            {
                review.Update(dto.Title, dto.Message, dto.Stars);
                await _repo.SaveChangesAsync(cancellationToken);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                throw new InvalidOperationException($"Failed to update review: {ex.Message}", ex);
            }
            catch (ArgumentException ex)
            {
                throw new InvalidOperationException($"Failed to update review: {ex.Message}", ex);
            }
        }

        public async Task DeleteReviewAsync(
            int id,
            int currentUserId,
            CancellationToken cancellationToken)
        {
            var review = await _repo.GetByIdAsync(id, cancellationToken);

            if (review == null)
                throw new KeyNotFoundException($"Review with ID {id} not found");

            if (review.ReviewerId != currentUserId)
                throw new UnauthorizedAccessException("You can only delete your own reviews");

            review.Delete();
            await _repo.SaveChangesAsync(cancellationToken);
        }
    }
}