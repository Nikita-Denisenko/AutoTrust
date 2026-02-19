using AutoTrust.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace AutoTrust.Application.Models.DTOs.Requests.UpdateDtos
{
    public record UpdateUserInfoDto
    (
        DateOnly? BirthDate,

        [Url]
        string? AvatarUrl,

        Gender? Gender,

        [MinLength(2)]
        [MaxLength(2500)]
        string? AboutInfo,

        [Range(1, int.MaxValue)]
        int? CountryId,

        [Range(1, int.MaxValue)]
        int? CityId
    );
}
