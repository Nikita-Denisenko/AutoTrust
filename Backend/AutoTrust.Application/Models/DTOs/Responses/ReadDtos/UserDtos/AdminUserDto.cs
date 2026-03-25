using AutoTrust.Application.Models.DTOs.Responses.ReadDtos.LocationDTOs;
using AutoTrust.Domain.Enums;

namespace AutoTrust.Application.Models.DTOs.Responses.ReadDtos.UserDtos
{
    public record AdminUserDto
    (
        int Id,
        string Name,
        string Surname,
        string Patronymic,
        DateOnly BirthDate,
        string? AvatarUrl,
        Gender Gender,
        string AboutInfo,
        LocationDto Location,
        bool IsDeleted,
        bool IsBlocked
    );
}