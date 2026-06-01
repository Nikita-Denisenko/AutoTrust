using AutoTrust.Application.Models.DTOs.Responses.ReadDtos.UserDtos;

namespace AutoTrust.Application.Models.DTOs.Responses.ReadDtos.ReactionDtos
{
    public record ReactionDto
    {
        public int Id { get; init; }
        public string Emoji { get; init; }
        public UserShortDto User { get; init; }
        public DateTime CreatedAt { get; init; }
    };
}
