using AutoTrust.Application.Models.DTOs.Responses.ReadDtos.LocationDTOs;
using AutoTrust.Application.Models.DTOs.Responses.ReadDtos.UserDtos;
using AutoTrust.Domain.Enums;

namespace AutoTrust.Application.Models.DTOs.Responses.ReadDtos.ListingDtos.BuyListingDtos
{
    public class BuyListingDto
    {
        public int Id { get; set; }
        public UserShortDto Author { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public LocationDto Location { get; set; }
        public int ModelId { get; set; }
        public string ModelName { get; set; }
        public string BrandImageUrl { get; set; }
        public decimal MinPrice { get; set; }
        public decimal MaxPrice { get; set; }
        public int MinReleaseYear { get; set; }
        public int MaxReleaseYear { get; set; }
        public CarColor? CarColor { get; set; }
        public int ReactionsQuantity { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
    }
}