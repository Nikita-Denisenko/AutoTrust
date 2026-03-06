using AutoTrust.Application.Models.DTOs.Responses.ReadDtos.ListingDtos;
using AutoTrust.Application.Models.DTOs.Responses.ReadDtos.UserDtos;

namespace AutoTrust.Application.Models.DTOs.Responses.ReadDtos.CommentDtos
{
    public record AdminUserCommentDto
    (
        int Id,
        ListingShortDto Listing,
        string Text,
        DateTime CreatedAt,
        bool IsBlocked,
        bool IsDeleted
    );
}
