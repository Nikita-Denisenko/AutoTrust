using AutoTrust.Application.Models.DTOs.Responses.ReadDtos.UserDtos;

namespace AutoTrust.Application.Models.DTOs.Responses.ReadDtos.ReactionDtos
{
    public record AdminListingReactionDto
    {
        public int Id { get; init; }
        public string Emoji { get; init; }
        public UserShortDto User { get; init; }
        public DateTime CreatedAt { get; init; }
        public bool IsDeleted { get; init; }
    };
}
