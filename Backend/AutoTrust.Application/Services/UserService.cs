using AutoMapper;
using AutoMapper.QueryableExtensions;
using AutoTrust.Application.Interfaces.Repositories;
using AutoTrust.Application.Interfaces.Services;
using AutoTrust.Application.Interfaces.Validators;
using AutoTrust.Application.Models.DTOs.Requests.FilterDtos.User;
using AutoTrust.Application.Models.DTOs.Requests.UpdateDtos.User;
using AutoTrust.Application.Models.DTOs.Responses.ReadDtos.UserDtos;
using AutoTrust.Domain.Entities;
using AutoTrust.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace AutoTrust.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IRepository<User> _repo;
        private readonly IUserValidator _userValidator;
        private readonly IMapper _mapper;

        public UserService(
            IRepository<User> repo,
            IUserValidator userValidator,
            IMapper mapper)
        {
            _repo = repo;
            _userValidator = userValidator;
            _mapper = mapper;
        }

        private IQueryable<User> ApplyFilters(UserFilterDto filterDto, bool isAdmin = false)
        {
            var query = _repo.GetQuery().AsNoTracking();

            if (filterDto is AdminUserFilterDto adminDto && adminDto.IsDeleted != null)
                query = query.Where(u => u.IsDeleted == adminDto.IsDeleted.Value);
            else if (!isAdmin)
                query = query.Where(u => !u.IsDeleted);

            if (filterDto is AdminUserFilterDto adminFilter && adminFilter.IsBlocked != null)
                query = query.Where(u => u.IsBlocked == adminFilter.IsBlocked.Value);

            if (filterDto.CityId.HasValue)
                query = query.Where(u => u.CityId == filterDto.CityId.Value);

            if (!string.IsNullOrWhiteSpace(filterDto.SearchText))
                query = query.Where(u =>
                    (u.Name + " " + u.Surname + " " + u.Patronymic).Contains(filterDto.SearchText));

            query = filterDto.SortByAsc
                ? query.OrderBy(u => u.Name)
                : query.OrderByDescending(u => u.Name);

            query = query
                .Skip((filterDto.Page - 1) * filterDto.Size)
                .Take(filterDto.Size);

            return query;
        }

        public async Task<UserProfileDto> GetUserByIdAsync(
            int id,
            CancellationToken cancellationToken)
        {
            var user = await _repo.GetQuery()
                .AsNoTracking()
                .Where(u => u.Id == id && !u.IsDeleted)
                .ProjectTo<UserProfileDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(cancellationToken);

            if (user == null)
                throw new KeyNotFoundException($"User with ID {id} not found");

            return user;
        }

        public async Task<AdminUserDto> GetUserByIdForAdminAsync(
            int id,
            CancellationToken cancellationToken)
        {
            var user = await _repo.GetQuery()
                .AsNoTracking()
                .Where(u => u.Id == id)
                .ProjectTo<AdminUserDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(cancellationToken);

            if (user == null)
                throw new KeyNotFoundException($"User with ID {id} not found");

            return user;
        }

        public async Task<List<UserProfileListItemDto>> GetUsersAsync(
            UserFilterDto filterDto,
            CancellationToken cancellationToken)
        {
            var query = ApplyFilters(filterDto, isAdmin: false);
            return await _mapper
                .ProjectTo<UserProfileListItemDto>(query)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<AdminUserListItemDto>> GetUsersForAdminAsync(
            AdminUserFilterDto filterDto,
            CancellationToken cancellationToken)
        {
            var query = ApplyFilters(filterDto, isAdmin: true);
            return await _mapper
                .ProjectTo<AdminUserListItemDto>(query)
                .ToListAsync(cancellationToken);
        }

        public async Task UpdateUserAvatarAsync(
            int currentUserId,
            UpdateAvatarUrlDto dto,
            CancellationToken cancellationToken)
        {
            var user = await _repo.GetByIdAsync(currentUserId, cancellationToken);
            if (user == null)
                throw new KeyNotFoundException($"User with ID {currentUserId} not found");

            try
            {
                user.ChangeAvatar(string.IsNullOrWhiteSpace(dto.AvatarUrl)
                    ? null
                    : Url.Create(dto.AvatarUrl));
                await _repo.SaveChangesAsync(cancellationToken);
            }
            catch (ArgumentException ex)
            {
                throw new InvalidOperationException($"Failed to update avatar: {ex.Message}", ex);
            }
        }

        public async Task UpdateUserInfoAsync(
            int currentUserId,
            UpdateUserInfoDto dto,
            CancellationToken cancellationToken)
        {
            var validationResult = await _userValidator.CanUpdateAsync(dto, cancellationToken);
            if (!validationResult.IsValid)
                throw new InvalidOperationException($"Failed to update user info: {validationResult.ErrorMessage}");

            var user = await _repo.GetByIdAsync(currentUserId, cancellationToken);
            if (user == null)
                throw new KeyNotFoundException($"User with ID {currentUserId} not found");

            try
            {
                user.UpdateInfo(
                    dto.Name,
                    dto.Surname,
                    dto.Patronymic,
                    dto.BirthDate != null ? BirthDate.Create(dto.BirthDate.Value) : null,
                    dto.Gender,
                    dto.AboutInfo,
                    dto.CityId
                );
                await _repo.SaveChangesAsync(cancellationToken);
            }
            catch (ArgumentException ex)
            {
                throw new InvalidOperationException($"Failed to update user info: {ex.Message}", ex);
            }
        }

        public async Task DeleteUserProfileAsync(
            int id,
            CancellationToken cancellationToken)
        {
            var user = await _repo.GetByIdAsync(id, cancellationToken);
            if (user == null)
                throw new KeyNotFoundException($"User with ID {id} not found");

            user.Delete();
            await _repo.SaveChangesAsync(cancellationToken);
        }

        public async Task BlockUserAsync(
            int id,
            CancellationToken cancellationToken)
        {
            var user = await _repo.GetByIdAsync(id, cancellationToken);
            if (user == null)
                throw new KeyNotFoundException($"User with ID {id} not found");

            user.Block();
            await _repo.SaveChangesAsync(cancellationToken);
        }

        public async Task UnblockUserAsync(
            int id,
            CancellationToken cancellationToken)
        {
            var user = await _repo.GetByIdAsync(id, cancellationToken);
            if (user == null)
                throw new KeyNotFoundException($"User with ID {id} not found");

            user.UnBlock();
            await _repo.SaveChangesAsync(cancellationToken);
        }
    }
}
