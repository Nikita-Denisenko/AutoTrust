using AutoTrust.Application.Models.DTOs.Responses.ReadDtos.ListingDtos;
using AutoTrust.Application.Models.DTOs.Responses.ReadDtos.UserDtos;

namespace AutoTrust.Application.Models.DTOs.Responses.ReadDtos.ReactionDtos
{
    public record AdminUserReactionDto
    (
        int Id,
        string Emoji,
        ListingShortDto Listing,
        DateTime CreatedAt,
        bool IsDeleted
    );
}
