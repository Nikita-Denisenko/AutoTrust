using AutoTrust.Application.Models.DTOs.Responses.ReadDtos.UserDtos;

namespace AutoTrust.Application.Models.DTOs.Responses.ReadDtos.CommentDtos
{
    public record AdminListingCommentDto
    (
        int Id,
        UserShortDto User,
        string Text,
        DateTime CreatedAt,
        bool IsBlocked,
        bool IsDeleted
    );
}
