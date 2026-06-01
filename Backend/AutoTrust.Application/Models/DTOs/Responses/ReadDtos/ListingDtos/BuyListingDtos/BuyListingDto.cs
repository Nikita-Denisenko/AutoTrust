using AutoTrust.Application.Models.DTOs.Responses.ReadDtos.LocationDTOs;
using AutoTrust.Application.Models.DTOs.Responses.ReadDtos.UserDtos;
using AutoTrust.Domain.Enums;

namespace AutoTrust.Application.Models.DTOs.Responses.ReadDtos.ListingDtos.BuyListingDtos
{
    public record BuyListingDto
    {
        public int Id { get; init; }
        public UserShortDto Author { get; init; }
        public DateTime CreatedAt { get; init; }
        public DateTime? UpdatedAt { get; init; }
        public LocationDto Location { get; init; }
        public int ModelId { get; init; }
        public string ModelName { get; init; }
        public string BrandImageUrl { get; init; }
        public decimal MinPrice { get; init; }
        public decimal MaxPrice { get; init; }
        public int MinReleaseYear { get; init; }
        public int MaxReleaseYear { get; init; }
        public CarColor? CarColor { get; init; }
        public int ReactionsQuantity { get; init; }
        public string Description { get; init; }
        public bool IsActive { get; init; }
    }
}