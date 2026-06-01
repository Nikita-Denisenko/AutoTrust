using AutoTrust.Application.Models.DTOs.Responses.ReadDtos.ListingDtos;
using AutoTrust.Application.Models.DTOs.Responses.ReadDtos.UserDtos;

namespace AutoTrust.Application.Models.DTOs.Responses.ReadDtos.ReactionDtos
{
    public record AdminUserReactionDto
    {
        public int Id { get; init; }
        public string Emoji { get; init; }
        public ListingShortDto Listing { get; init; }
        public DateTime CreatedAt { get; init; }
        public bool IsDeleted { get; init; }
    };
}
