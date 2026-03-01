using AutoTrust.Application.Models.DTOs.Responses.ReadDtos.CarDtos;
using AutoTrust.Application.Models.DTOs.Responses.ReadDtos.LocationDTOs;
using AutoTrust.Application.Models.DTOs.Responses.ReadDtos.UserDtos;
using AutoTrust.Domain.Enums;

namespace AutoTrust.Application.Models.DTOs.Responses.ReadDtos.ListingDtos.SaleListingDtos
{
    public record SaleListingDto
    (
        int Id,
        UserShortDto Author,
        DateTime CreatedAt,
        LocationDto Location,
        decimal Price,
        PublicCarDto Car,
        CarColor? CarColor,
        int ReactionsQuantity
    );
}
