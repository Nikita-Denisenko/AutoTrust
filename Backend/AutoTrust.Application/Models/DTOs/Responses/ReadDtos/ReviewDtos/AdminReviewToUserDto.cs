using AutoTrust.Application.Models.DTOs.Responses.ReadDtos.UserDtos;

namespace AutoTrust.Application.Models.DTOs.Responses.ReadDtos.ReviewDtos
{
    public record AdminReviewToUserDto
    (
        int Id,
        string Title,
        string Message,
        int Stars,
        UserShortDto Reviewer,
        DateTime CreatedAt,
        bool IsDeleted
    );
}
