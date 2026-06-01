namespace AutoTrust.Application.Models.DTOs.Responses.ReadDtos.NotificationDtos
{
    public record NotificationDto
    {
        public string Title { get; init; }
        public string Text { get; init; }
        public bool IsRead { get; init; }
        public DateTime CreatedAt { get; init; }
    };
}
