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
        LocationDto Location,
        int ModelId,
        string ModelName,
        decimal MinPrice,
        decimal MaxPrice,
        int MinReleaseYear,
        int MaxReleaseYear,
        CarColor? CarColor,
        int ReactionsQuantity
    );
}
