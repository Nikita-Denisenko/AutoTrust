using AutoTrust.Application.Models.DTOs.Responses.ReadDtos.UserDtos;

namespace AutoTrust.Application.Models.DTOs.Responses.ReadDtos.MessageDtos
{
    public record AdminMessageDto
    {
        public int Id { get; init; }
        public string Text { get; init; }
        public int ChatId { get; init; }
        public bool IsRead { get; init; }
        public DateTime SentAt { get; init; }
        public UserShortDto User { get; init; }
        public bool IsDeleted { get; init; }
    };
}
