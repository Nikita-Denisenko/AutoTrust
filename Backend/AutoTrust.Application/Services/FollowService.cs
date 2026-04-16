using AutoMapper;
using AutoTrust.Application.Interfaces.Repositories;
using AutoTrust.Application.Interfaces.Services;
using AutoTrust.Application.Interfaces.Validators;
using AutoTrust.Application.Models.DTOs.Requests.CreateDtos;
using AutoTrust.Application.Models.DTOs.Requests.FilterDtos.Follow;
using AutoTrust.Application.Models.DTOs.Responses.CreatedDtos;
using AutoTrust.Application.Models.DTOs.Responses.ReadDtos.FollowDtos;
using AutoTrust.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AutoTrust.Application.Services
{
    public class FollowService : IFollowService
    {
        private readonly IFollowValidator _validator;
        private readonly IRepository<Follow> _repo;
        private readonly IMapper _mapper;

        public FollowService(IFollowValidator validator, IRepository<Follow> repo, IMapper mapper)
        {
            _validator = validator;
            _repo = repo;
            _mapper = mapper;
        }

        private IQueryable<Follow> ApplyFilters
        (
            int currentUserId, 
            FollowFilterDto filterDto,
            bool followers)
        {
            var query = _repo
                .GetQuery()
                .AsNoTracking()
                .Where(f => followers ? f.TargetId == currentUserId : f.FollowerId == currentUserId);

            query = followers
                ? query.Where(f => (f.Follower.Name + f.Follower.Surname).ToLower().Contains(filterDto.SearchText.ToLower()))
                : query.Where(f => (f.Target.Name + f.Target.Surname).ToLower().Contains(filterDto.SearchText.ToLower()));

             query = query   
                .Skip((filterDto.Page - 1) * filterDto.Size)
                .Take(filterDto.Size);

            return query;
        }

        public async Task<CreatedFollowDto> CreateFollowAsync(int currentUserId, CreateFollowDto dto, CancellationToken cancellationToken)
        {
            var (isValid, error) = await _validator.CanCreateAsync(currentUserId, dto, cancellationToken);

            if (!isValid)
                throw new InvalidOperationException($"Failed to create follow: {error}");

            try
            {
                var follow = _mapper.Map<Follow>(dto, opts => opts.Items["FollowerId"] = currentUserId);

                await _repo.AddAsync(follow, cancellationToken);
                await _repo.SaveChangesAsync(cancellationToken);

                return _mapper.Map<CreatedFollowDto>(follow);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                throw new InvalidOperationException($"Failed to create follow: {ex.Message}", ex);
            }
        }

        public async Task DeleteFollowAsync(int id, int currentUserId, CancellationToken cancellationToken)
        {
            var follow = await _repo.GetByIdAsync(id, cancellationToken);

            if (follow == null)
                throw new KeyNotFoundException($"Follow with ID {id} was not found to delete!");

            if (follow.FollowerId != currentUserId)
                throw new InvalidOperationException($"User cannot delete other users follows!");

            _repo.Delete(follow);

            await _repo.SaveChangesAsync(cancellationToken);
        }

        public async Task<List<UserFollowerDto>> GetUserFollowersAsync
        (
            int currentUserId, 
            FollowFilterDto filterDto, 
            CancellationToken cancellationToken
        )
        {
            var followers = ApplyFilters(currentUserId, filterDto, true);

            return await _mapper
                .ProjectTo<UserFollowerDto>(followers)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<UserFollowDto>> GetUserFollowsAsync
        (
            int currentUserId, 
            FollowFilterDto filterDto, 
            CancellationToken cancellationToken
        )
        {
            var followers = ApplyFilters(currentUserId, filterDto, false);

            return await _mapper
                .ProjectTo<UserFollowDto>(followers)
                .ToListAsync(cancellationToken);
        }
    }
}
