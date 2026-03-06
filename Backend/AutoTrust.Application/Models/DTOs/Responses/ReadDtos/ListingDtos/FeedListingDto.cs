using AutoTrust.Application.Models.DTOs.Responses.ReadDtos.ListingDtos.BuyListingDtos;
using AutoTrust.Application.Models.DTOs.Responses.ReadDtos.ListingDtos.SaleListingDtos;
using AutoTrust.Application.Models.DTOs.Responses.ReadDtos.LocationDTOs;
using AutoTrust.Application.Models.DTOs.Responses.ReadDtos.UserDtos;
using AutoTrust.Domain.Enums;

namespace AutoTrust.Application.Models.DTOs.Responses.ReadDtos.ListingDtos
{
    public record FeedListingDto
    (
        int Id,
        string Name,
        UserShortDto Author,
        ListingType Type,
        DateTime CreatedAt,
        LocationDto Location,
        BuyInfoDto? BuyInfoDto,
        SaleInfoDto? SaleInfoDto,
        int ReactionsQuantity
    );
}
