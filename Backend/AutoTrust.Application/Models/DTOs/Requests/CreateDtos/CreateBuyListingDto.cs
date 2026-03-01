
using AutoTrust.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace AutoTrust.Application.Models.DTOs.Requests.CreateDtos
{
    public record CreateBuyListingDto
    (
        [Required]
        [MaxLength(40)]
        string Name,

        [Required]
        [Range(1, int.MaxValue)]
        int UserId,

        [Required]
        [MinLength(1)]
        [MaxLength(4500)]
        string Description,

        [Required]
        [Range(1, int.MaxValue)]
        int CountryId,

        [Required]
        [Range(1, int.MaxValue)]
        int CityId,

        [Required]
        [Range(1, int.MaxValue)]
        int ModelId,

        [Range(0, int.MaxValue)]
        decimal? MinPrice,

        [Range(0, int.MaxValue)]
        decimal? MaxPrice,

        [Range(1900, int.MaxValue)]
        int? MinReleaseYear,

        [Range(1900, int.MaxValue)]
        int? MaxReleaseYear,

        CarColor? CarColor
    );
}
