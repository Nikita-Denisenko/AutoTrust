using AutoTrust.Application.Models.DTOs.Responses.ReadDtos.UserDtos;

namespace AutoTrust.Application.Models.DTOs.Responses.ReadDtos.ReviewDtos
{
    public record AdminReviewToUserListItemDto
    (
        int Id,
        string Title,
        int Stars,
        UserShortDto Reviewer,
        DateTime CreatedAt,
        bool IsDeleted
    );
}
