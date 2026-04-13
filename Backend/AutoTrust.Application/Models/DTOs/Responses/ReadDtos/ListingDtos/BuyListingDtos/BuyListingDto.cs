using AutoTrust.Application.Models.DTOs.Responses.ReadDtos.LocationDTOs;
using AutoTrust.Application.Models.DTOs.Responses.ReadDtos.UserDtos;
using AutoTrust.Domain.Enums;

namespace AutoTrust.Application.Models.DTOs.Responses.ReadDtos.ListingDtos.BuyListingDtos
{
    public record BuyListingDto
    (
        int Id,
        UserShortDto Author,
        DateTime CreatedAt,
        DateTime? UpdatedAt,
        LocationDto Location,
        int ModelId,
        string ModelName,
        string ModelImageUrl,
        decimal MinPrice,
        decimal MaxPrice,
        int MinReleaseYear,
        int MaxReleaseYear,
        CarColor? CarColor,
        int ReactionsQuantity,
        string Description
    );
}
