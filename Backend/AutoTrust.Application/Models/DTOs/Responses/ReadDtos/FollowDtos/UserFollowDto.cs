using AutoTrust.Application.Models.DTOs.Responses.ReadDtos.UserDtos;

namespace AutoTrust.Application.Models.DTOs.Responses.ReadDtos.FollowDtos
{
    public record UserFollowDto
    (
        int Id,
        UserShortDto Target
    );
}