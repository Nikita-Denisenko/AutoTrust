using AutoTrust.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace AutoTrust.Application.Models.DTOs.Requests.UpdateDtos.Listing
{
    public record UpdateBuyListingDto
    (
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
