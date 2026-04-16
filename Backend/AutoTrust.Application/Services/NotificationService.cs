using AutoMapper;
using AutoMapper.QueryableExtensions;
using AutoTrust.Application.Interfaces.Repositories;
using AutoTrust.Application.Interfaces.Services;
using AutoTrust.Application.Interfaces.Validators;
using AutoTrust.Application.Models.DTOs.Requests.Actions.Notification;
using AutoTrust.Application.Models.DTOs.Requests.CreateDtos;
using AutoTrust.Application.Models.DTOs.Requests.FilterDtos.Notification;
using AutoTrust.Application.Models.DTOs.Requests.UpdateDtos.Notification;
using AutoTrust.Application.Models.DTOs.Responses.CreatedDtos;
using AutoTrust.Application.Models.DTOs.Responses.ReadDtos.NotificationDtos;
using AutoTrust.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AutoTrust.Application.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IRepository<Notification> _repo;
        private readonly INotificationValidator _notificationValidator;
        private readonly IMapper _mapper;

        public NotificationService(
            IRepository<Notification> repo,
            INotificationValidator notificationValidator,
            IMapper mapper)
        {
            _repo = repo;
            _notificationValidator = notificationValidator;
            _mapper = mapper;
        }

        private IQueryable<Notification> ApplyFilters
        (
            NotificationFilterDto filterDto,
            int? userId = null,
            bool isAdmin = false
        )
        {
            var query = _repo.GetQuery().AsNoTracking();

            if (filterDto is AdminNotificationFilterDto adminDto && adminDto.IsDeleted != null)
                query = query.Where(n => n.IsDeleted == adminDto.IsDeleted.Value);
            else if (!isAdmin)
                query = query.Where(n => !n.IsDeleted);

            if (userId.HasValue)
                query = query.Where(n => n.UserId == userId.Value);

            if (filterDto.IsRead.HasValue)
                query = query.Where(n => n.IsRead == filterDto.IsRead.Value);

            query = filterDto.SortByAsc
                ? query.OrderBy(n => n.CreatedAt)
                : query.OrderByDescending(n => n.CreatedAt);

            query = query
                .Skip((filterDto.Page - 1) * filterDto.Size)
                .Take(filterDto.Size);

            return query;
        }

        public async Task<CreatedNotificationDto> CreateNotificationAsync
        (
            CreateNotificationDto dto,
            CancellationToken cancellationToken
        )
        {
            var validationResult = await _notificationValidator.CanCreateAsync(dto, cancellationToken);

            if (!validationResult.IsValid)
                throw new InvalidOperationException($"Failed to create notification: {validationResult.ErrorMessage}");

            var notification = _mapper.Map<Notification>(dto);
            await _repo.AddAsync(notification, cancellationToken);
            await _repo.SaveChangesAsync(cancellationToken);

            return _mapper.Map<CreatedNotificationDto>(notification);
        }

        public async Task<NotificationDto> GetNotificationAsync
        (
            int id,
            int currentUserId,
            CancellationToken cancellationToken
        )
        {
            var notification = await _repo.GetQuery()
                .AsNoTracking()
                .Where(n => n.Id == id && n.UserId == currentUserId && !n.IsDeleted)
                .ProjectTo<NotificationDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(cancellationToken);

            if (notification == null)
                throw new KeyNotFoundException($"Notification with ID {id} not found or access denied");

            return notification;
        }

        public async Task<AdminNotificationDto> GetNotificationForAdminAsync
        (
            int id,
            CancellationToken cancellationToken
        )
        {
            var notification = await _repo.GetQuery()
                .AsNoTracking()
                .Where(n => n.Id == id)
                .ProjectTo<AdminNotificationDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(cancellationToken);

            if (notification == null)
                throw new KeyNotFoundException($"Notification with ID {id} not found");

            return notification;
        }

        public async Task<List<NotificationListItemDto>> GetNotificationsAsync
        (
            int currentUserId,
            NotificationFilterDto filterDto,
            CancellationToken cancellationToken
        )
        {
            var query = ApplyFilters(filterDto, currentUserId, isAdmin: false);
            return await _mapper
                .ProjectTo<NotificationListItemDto>(query)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<AdminNotificationListItemDto>> GetNotificationsForAdminAsync
        (
            AdminNotificationFilterDto filterDto,
            CancellationToken cancellationToken
        )
        {
            var query = ApplyFilters(filterDto, filterDto.UserId, isAdmin: true);
            return await _mapper
                .ProjectTo<AdminNotificationListItemDto>(query)
                .ToListAsync(cancellationToken);
        }

        public async Task UpdateNotificationAsync
        (
            int id,
            UpdateNotificationDto dto,
            CancellationToken cancellationToken
        )
        {
            var notification = await _repo.GetByIdAsync(id, cancellationToken);

            if (notification == null)
                throw new KeyNotFoundException($"Notification with ID {id} not found");

            try
            {
                notification.Update(dto.Title, dto.Text);
                await _repo.SaveChangesAsync(cancellationToken);
            }
            catch (ArgumentException ex)
            {
                throw new InvalidOperationException($"Failed to update notification: {ex.Message}", ex);
            }
        }

        public async Task MarkAsReadNotificationsAsync
        (
            int currentUserId,
            MarkAsReadNotificationsDto dto,
            CancellationToken cancellationToken
        )
        {
            var notifications = _repo.GetQuery()
                .Where(n => dto.NotificationIds.Contains(n.Id) && n.UserId == currentUserId && !n.IsDeleted);

            if (!await notifications.AnyAsync(cancellationToken))
                throw new InvalidOperationException("No valid notifications to mark as read");

            foreach (var notification in notifications)
                notification.MakeAsRead();

            await _repo.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteNotificationsAsync
        (
            int currentUserId,
            bool isAdmin,
            DeleteNotificationsDto dto,
            CancellationToken cancellationToken
        )
        {
            var query = _repo.GetQuery().Where(n => dto.NotificationIds.Contains(n.Id));

            if (!isAdmin)
                query = query.Where(n => n.UserId == currentUserId);

            var notifications = await query.ToListAsync(cancellationToken);

            if (!notifications.Any())
                throw new InvalidOperationException("No valid notifications to delete");

            foreach (var notification in notifications)
                notification.Delete();

            await _repo.SaveChangesAsync(cancellationToken);
        }
    }
}