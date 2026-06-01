using AutoTrust.Application.Models.DTOs.Responses.ReadDtos.UserDtos;

namespace AutoTrust.Application.Models.DTOs.Responses.ReadDtos.FollowDtos
{
    public record UserFollowDto
    {
        public int Id { get; init; }
        public UserShortDto Target { get; init; }
    };
}