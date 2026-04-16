using AutoTrust.Application.Models.DTOs.Requests.Actions.Notification;
using AutoTrust.Application.Models.DTOs.Requests.CreateDtos;
using AutoTrust.Application.Models.DTOs.Requests.FilterDtos.Notification;
using AutoTrust.Application.Models.DTOs.Requests.UpdateDtos.Notification;
using AutoTrust.Application.Models.DTOs.Responses.CreatedDtos;
using AutoTrust.Application.Models.DTOs.Responses.ReadDtos.NotificationDtos;

namespace AutoTrust.Application.Interfaces.Services
{
    public interface INotificationService
    {
        public Task<CreatedNotificationDto> CreateNotificationAsync(CreateNotificationDto dto, CancellationToken cancellationToken);
        public Task<NotificationDto> GetNotificationAsync(int id, int currentUserId, CancellationToken cancellationToken);
        public Task<AdminNotificationDto> GetNotificationForAdminAsync(int id, CancellationToken cancellationToken);
        public Task<List<NotificationListItemDto>> GetNotificationsAsync(int currentUserId,  NotificationFilterDto filterDto, CancellationToken cancellationToken);
        public Task<List<AdminNotificationListItemDto>> GetNotificationsForAdminAsync(AdminNotificationFilterDto filterDto, CancellationToken cancellationToken);
        public Task UpdateNotificationAsync(int id, UpdateNotificationDto dto, CancellationToken cancellationToken);
        public Task MarkAsReadNotificationsAsync(int currentUserId, MarkAsReadNotificationsDto dto, CancellationToken cancellationToken);
        public Task DeleteNotificationsAsync(int currentUserId, bool isAdmin, DeleteNotificationsDto dto, CancellationToken cancellationToken);
    }
}
