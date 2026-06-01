using AutoTrust.Application.Models.DTOs.Responses.ReadDtos.UserDtos;

namespace AutoTrust.Application.Models.DTOs.Responses.ReadDtos.FollowDtos
{
    public record UserFollowerDto
    {
        public int Id { get; init; }
        public UserShortDto Follower { get; init; }
    };
}
