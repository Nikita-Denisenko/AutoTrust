using AutoTrust.Application.Models.DTOs.Responses.ReadDtos.CarDtos;
using AutoTrust.Application.Models.DTOs.Responses.ReadDtos.LocationDTOs;
using AutoTrust.Application.Models.DTOs.Responses.ReadDtos.UserDtos;

namespace AutoTrust.Application.Models.DTOs.Responses.ReadDtos.ListingDtos.SaleListingDtos
{
    public record SaleListingDto
    {
        public int Id { get; init; }
        public UserShortDto Author { get; init; }
        public DateTime CreatedAt { get; init; }
        public DateTime? UpdatedAt { get; init; }
        public LocationDto Location { get; init; }
        public decimal Price { get; init; }
        public PublicCarDto? Car { get; init; }
        public string Description { get; init; }
        public int ReactionsQuantity { get; init; }
        public bool IsActive { get; init; }
    }
}