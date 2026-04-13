using AutoTrust.Application.Models.DTOs.Responses.ReadDtos.CarDtos;
using AutoTrust.Application.Models.DTOs.Responses.ReadDtos.LocationDTOs;
using AutoTrust.Application.Models.DTOs.Responses.ReadDtos.UserDtos;

namespace AutoTrust.Application.Models.DTOs.Responses.ReadDtos.ListingDtos.SaleListingDtos
{
    public record SaleListingDto
    (
        int Id,
        UserShortDto Author,
        DateTime CreatedAt,
        DateTime? UpdatedAt,
        LocationDto Location,
        decimal Price,
        PublicCarDto Car,
        string Description,
        int ReactionsQuantity
    );
}
