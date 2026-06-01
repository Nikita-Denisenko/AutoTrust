using AutoTrust.Application.Models.DTOs.Responses.ReadDtos.LocationDTOs;
using AutoTrust.Domain.Enums;

namespace AutoTrust.Application.Models.DTOs.Responses.ReadDtos.UserDtos
{
    public record AdminUserDto
    {
        public int Id { get; init; }
        public string Name { get; init; }
        public string Surname { get; init; }
        public string Patronymic { get; init; }
        public DateOnly BirthDate { get; init; }
        public string? AvatarUrl { get; init; }
        public Gender Gender { get; init; }
        public string AboutInfo { get; init; }
        public LocationDto Location { get; init; }
        public bool IsDeleted { get; init; }
        public bool IsBlocked { get; init; }
        public int ReviewsQuantity { get; init; }
        public int FollowersQuantity { get; init; }
        public int FollowingsQuantity { get; init; }
    };
}