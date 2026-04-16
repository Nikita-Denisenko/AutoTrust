namespace AutoTrust.Application.Models.DTOs.Requests.FilterDtos.Notification
{
    public record AdminNotificationFilterDto
    (
        bool? IsDeleted = null,
        int? UserId = null
    ) : NotificationFilterDto;
}
