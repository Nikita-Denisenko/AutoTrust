using AutoTrust.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace AutoTrust.Application.Models.DTOs.Requests.UpdateDtos.Car
{
    public record UpdateCarDto
    (
        [MaxLength(500)]
        string? Description,

        [Url]
        string? ImageUrl,

        CarColor? Color,

        [RegularExpression("^[А-Я]\\d{3}[А-Я]{2}\\d{2,3}$")]
        string? StateNumber,

        [Range(0, int.MaxValue)]
        decimal? EngineMileage,

        [Range(1, int.MaxValue)]
        int? LocationCityId,

        bool? HasAccident
    );
}
