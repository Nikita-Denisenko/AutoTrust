using AutoTrust.Application.Models.DTOs.Responses.ReadDtos.LocationDTOs;

namespace AutoTrust.Application.Models.DTOs.Responses.ReadDtos.UserDtos
{
    public record AdminUserListItemDto
    (
        int Id,
        string Name,
        DateOnly BirthDate,
        string? AvatarUrl,
        LocationDto Location,
        bool IsDeleted,
        bool IsBlocked
    );
}
