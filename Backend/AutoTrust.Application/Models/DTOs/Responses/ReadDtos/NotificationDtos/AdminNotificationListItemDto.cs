namespace AutoTrust.Application.Models.DTOs.Responses.ReadDtos.NotificationDtos
{
    public record AdminNotificationListItemDto
    (
        int Id,
        string Title,
        bool IsRead,
        DateTime CreatedAt,
        bool IdDeleted
    );
}
