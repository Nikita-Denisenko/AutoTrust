using AutoTrust.Application.Models.DTOs.Responses.ReadDtos.UserDtos;

namespace AutoTrust.Application.Models.DTOs.Responses.ReadDtos.ReviewDtos
{
    public record ReviewListItemDto
    (
        int Id,
        string Title,
        int Stars,
        UserShortDto Reviewer,
        DateTime CreatedAt
    );
}
