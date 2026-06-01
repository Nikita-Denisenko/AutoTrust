using AutoTrust.Application.Models.DTOs.Responses.ReadDtos.ListingDtos.BuyListingDtos;
using AutoTrust.Application.Models.DTOs.Responses.ReadDtos.ListingDtos.SaleListingDtos;
using AutoTrust.Application.Models.DTOs.Responses.ReadDtos.LocationDTOs;
using AutoTrust.Application.Models.DTOs.Responses.ReadDtos.UserDtos;
using AutoTrust.Domain.Enums;

namespace AutoTrust.Application.Models.DTOs.Responses.ReadDtos.ListingDtos
{
    public record class FeedListingDto
    {
        public int Id { get; init; }
        public string Name { get; init; }
        public UserShortDto Author { get; init; }
        public ListingType Type { get; init; }
        public DateTime CreatedAt { get; init; }
        public DateTime? UpdatedAt { get; init; }
        public LocationDto Location { get; init; }
        public BuyInfoDto? BuyInfoDto { get; init; }
        public SaleInfoDto? SaleInfoDto { get; init; }
        public string Description { get; init; }
        public int ReactionsQuantity { get; init; }
    }
}