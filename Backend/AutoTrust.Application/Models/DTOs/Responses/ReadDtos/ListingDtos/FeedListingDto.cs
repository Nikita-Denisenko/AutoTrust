using AutoTrust.Application.Models.DTOs.Responses.ReadDtos.ListingDtos.BuyListingDtos;
using AutoTrust.Application.Models.DTOs.Responses.ReadDtos.ListingDtos.SaleListingDtos;
using AutoTrust.Application.Models.DTOs.Responses.ReadDtos.LocationDTOs;
using AutoTrust.Application.Models.DTOs.Responses.ReadDtos.UserDtos;
using AutoTrust.Domain.Enums;

namespace AutoTrust.Application.Models.DTOs.Responses.ReadDtos.ListingDtos
{
    public record class FeedListingDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public UserShortDto Author { get; set; }
        public ListingType Type { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public LocationDto Location { get; set; }
        public BuyInfoDto? BuyInfoDto { get; set; }
        public SaleInfoDto? SaleInfoDto { get; set; }
        public string Description { get; set; }
        public int ReactionsQuantity { get; set; }
    }
}