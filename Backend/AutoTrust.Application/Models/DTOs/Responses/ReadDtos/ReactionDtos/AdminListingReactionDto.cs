using AutoTrust.Application.Models.DTOs.Responses.ReadDtos.UserDtos;

namespace AutoTrust.Application.Models.DTOs.Responses.ReadDtos.ReactionDtos
{
    public record AdminListingReactionDto
    (
        int Id,
        string Emoji,
        UserShortDto User,
        DateTime CreatedAt,
        bool IsDeleted
    );
}
