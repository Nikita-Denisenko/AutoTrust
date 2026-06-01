using AutoTrust.Application.Models.DTOs.Responses.ReadDtos.LocationDTOs;

namespace AutoTrust.Application.Models.DTOs.Responses.ReadDtos.UserDtos
{
    public record AdminUserListItemDto
    {
        public int Id { get; init; }
        public string Name { get; init; }
        public string Surname { get; init; }
        public DateOnly BirthDate { get; init; }
        public string? AvatarUrl { get; init; }
        public LocationDto Location { get; init; }
        public bool IsDeleted { get; init; }
        public bool IsBlocked { get; init; }
        public int FollowersQuantity { get; init; }
    };
}
