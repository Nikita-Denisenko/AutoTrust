using AutoTrust.Application.Models.DTOs.Responses.ReadDtos.UserDtos;

namespace AutoTrust.Application.Models.DTOs.Responses.ReadDtos.CommentDtos
{
    public record CommentDto
    (
        int Id,
        UserShortDto User,
        string Text,
        DateTime CreatedAt,
        bool IsBlocked
    );
}
