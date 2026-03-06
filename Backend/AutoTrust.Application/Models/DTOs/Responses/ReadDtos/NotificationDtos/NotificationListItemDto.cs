namespace AutoTrust.Application.Models.DTOs.Responses.ReadDtos.NotificationDtos
{
    public record NotificationListItemDto
    (
        int Id,
        string Title,
        bool IsRead,
        DateTime CreatedAt
    );
}