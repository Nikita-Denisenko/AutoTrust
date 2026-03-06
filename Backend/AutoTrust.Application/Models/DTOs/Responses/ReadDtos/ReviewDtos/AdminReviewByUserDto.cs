using AutoTrust.Application.Models.DTOs.Responses.ReadDtos.UserDtos;

namespace AutoTrust.Application.Models.DTOs.Responses.ReadDtos.ReviewDtos
{
    public record AdminReviewByUserDto
    (
        int Id,
        string Title,
        string Message,
        int Stars,
        UserShortDto Receiver,
        DateTime CreatedAt,
        bool IsDeleted
    );
}
