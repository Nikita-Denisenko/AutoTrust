using AutoTrust.Application.Models.DTOs.Responses.ReadDtos.ListingDtos;
using AutoTrust.Application.Models.DTOs.Responses.ReadDtos.UserDtos;

namespace AutoTrust.Application.Models.DTOs.Responses.ReadDtos.CommentDtos
{
    public record AdminCommentDto
    (
        int Id,
        UserShortDto User,
        ListingShortDto Listing,
        string Text,
        DateTime CreatedAt,
        bool IsBlocked,
        bool IsDeleted
    );
}
