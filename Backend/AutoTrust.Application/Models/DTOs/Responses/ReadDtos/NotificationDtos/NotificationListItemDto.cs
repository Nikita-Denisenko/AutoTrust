namespace AutoTrust.Application.Models.DTOs.Responses.ReadDtos.NotificationDtos
{
    public record NotificationListItemDto
    {
        public int Id { get; init; }
        public string Title { get; init; }
        public bool IsRead { get; init; }
        public DateTime CreatedAt { get; init; }
    };
}