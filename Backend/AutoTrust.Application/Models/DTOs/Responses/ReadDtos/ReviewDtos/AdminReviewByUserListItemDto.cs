using AutoTrust.Application.Models.DTOs.Responses.ReadDtos.UserDtos;

namespace AutoTrust.Application.Models.DTOs.Responses.ReadDtos.ReviewDtos
{
    public record AdminReviewByUserListItemDto
    (
        int Id,
        string Title,
        int Stars,
        UserShortDto Receiver,
        DateTime CreatedAt,
        bool IsDeleted
    );
}
