namespace AutoTrust.Application.Models.DTOs.Responses.ReadDtos.NotificationDtos
{
    public record AdminNotificationDto
    (
        int Id,
        string Title,
        string Text,
        bool IsRead,
        DateTime CreatedAt,
        bool IdDeleted
    );
}
