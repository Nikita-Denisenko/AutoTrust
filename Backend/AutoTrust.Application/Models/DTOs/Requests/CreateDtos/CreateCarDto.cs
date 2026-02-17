using AutoTrust.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace AutoTrust.Application.Models.DTOs.Requests.CreateDtos
{
    public record CreateCarDto
    (
        [Required]
        [MaxLength(500)]
        string Description,

        [Required]
        [Range(1900, int.MaxValue)]
        int ReleaseYear,

        [Required]
        [Url]
        string ImageUrl,

        [Required]
        CarColor Color,

        [Required]
        [RegularExpression("^[А-Я]\\d{3}[А-Я]{2}\\d{2,3}$")]
        string StateNumber,

        [Required]
        [Range(0, int.MaxValue)]
        decimal EngineMileage,

        [Required]
        [Range(1, int.MaxValue)]
        int ModelId,

        [Required]
        [Range(1, int.MaxValue)]
        int LocationCityId,

        [Required]
        [Range(1, int.MaxValue)]
        int LocationCountryId,

        [Required]
        bool HasAccident
    );
}