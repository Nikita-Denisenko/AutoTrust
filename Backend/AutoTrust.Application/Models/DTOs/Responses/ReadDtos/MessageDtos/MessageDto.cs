using AutoTrust.Application.Models.DTOs.Responses.ReadDtos.UserDtos;

namespace AutoTrust.Application.Models.DTOs.Responses.ReadDtos.MessageDtos
{
    public record MessageDto
    {
        public int Id { get; init; }
        public string Text { get; init; }
        public UserShortDto User { get; init; }
        public bool IsRead { get; init; }
        public DateTime SentAt { get; init; }
    };
}