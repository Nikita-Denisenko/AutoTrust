using AutoMapper;
using AutoMapper.QueryableExtensions;
using AutoTrust.Application.Interfaces.Repositories;
using AutoTrust.Application.Interfaces.Services;
using AutoTrust.Application.Interfaces.Validators;
using AutoTrust.Application.Models.DTOs.Requests.CreateDtos;
using AutoTrust.Application.Models.DTOs.Requests.FilterDtos.Reaction;
using AutoTrust.Application.Models.DTOs.Responses.CreatedDtos;
using AutoTrust.Application.Models.DTOs.Responses.ReadDtos.ReactionDtos;
using AutoTrust.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AutoTrust.Infrastructure.Services
{
    public class ReactionService : IReactionService
    {
        private readonly IRepository<Reaction> _repo;
        private readonly IReactionValidator _validator;
        private readonly IMapper _mapper;

        public ReactionService(
            IRepository<Reaction> repo,
            IReactionValidator validator,
            IMapper mapper)
        {
            _repo = repo;
            _validator = validator;
            _mapper = mapper;
        }

        public async Task<CreatedReactionDto> CreatedReactionAsync(
            int currentUserId,
            CreateReactionDto dto,
            CancellationToken cancellationToken)
        {
            var validationResult = await _validator.CanCreateAsync(currentUserId, dto, cancellationToken);
            if (!validationResult.IsValid)
                throw new InvalidOperationException(validationResult.ErrorMessage);

            try
            {
                var reaction = _mapper.Map<Reaction>(dto, opts =>
                {
                    opts.Items["UserId"] = currentUserId;
                });

                await _repo.AddAsync(reaction, cancellationToken);
                await _repo.SaveChangesAsync(cancellationToken);

                return _mapper.Map<CreatedReactionDto>(reaction);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                throw new InvalidOperationException($"Failed to create reaction: {ex.Message}", ex);
            }
            catch (ArgumentException ex)
            {
                throw new InvalidOperationException($"Failed to create reaction: {ex.Message}", ex);
            }
        }

        public async Task<List<ReactionDto>> GetReactionsAsync(
            int listingId,
            ReactionFilterDto filterDto,
            CancellationToken cancellationToken)
        {
            return await _repo.GetQuery()
                .Where(r => r.ListingId == listingId && !r.IsDeleted)
                .Skip((filterDto.Page - 1) * filterDto.Size)
                .Take(filterDto.Size)
                .ProjectTo<ReactionDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<AdminListingReactionDto>> GetListingReactionsAsync(
            int listingId,
            AdminReactionFilterDto filterDto,
            CancellationToken cancellationToken)
        {
            var query = _repo.GetQuery()
                .Where(r => r.ListingId == listingId);

            if (filterDto.IsDeleted.HasValue)
                query = query.Where(r => r.IsDeleted == filterDto.IsDeleted.Value);

            query = filterDto.SortByAsc == true
                ? query.OrderBy(r => r.CreatedAt)
                : query.OrderByDescending(r => r.CreatedAt);

            query = query
                .Skip((filterDto.Page - 1) * filterDto.Size)
                .Take(filterDto.Size);

            return await query
                .ProjectTo<AdminListingReactionDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<AdminUserReactionDto>> GetUserReactionsAsync(
            int userId,
            AdminReactionFilterDto filterDto,
            CancellationToken cancellationToken)
        {
            var query = _repo.GetQuery()
                .Where(r => r.UserId == userId);

            if (filterDto.IsDeleted.HasValue)
                query = query.Where(r => r.IsDeleted == filterDto.IsDeleted.Value);

            query = filterDto.SortByAsc == true
                ? query.OrderBy(r => r.CreatedAt)
                : query.OrderByDescending(r => r.CreatedAt);

            query = query
                .Skip((filterDto.Page - 1) * filterDto.Size)
                .Take(filterDto.Size);

            return await query
                .ProjectTo<AdminUserReactionDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);
        }

        public async Task DeleteReactionAsync(
            int id,
            int currentUserId,
            CancellationToken cancellationToken)
        {
            var reaction = await _repo.GetByIdAsync(id, cancellationToken);

            if (reaction == null)
                throw new KeyNotFoundException($"Reaction with Id {id} was not found");

            if (reaction.UserId != currentUserId)
                throw new InvalidOperationException("You can only delete your own reactions");

            reaction.Delete();

            await _repo.SaveChangesAsync(cancellationToken);
        }
    }
}