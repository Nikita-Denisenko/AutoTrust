using AutoTrust.Domain.Enums;

namespace AutoTrust.Application.Models.DTOs.Responses.ReadDtos.ListingDtos.BuyListingDtos
{
    public record BuyInfoDto
    {
        public int ModelId { get; init; }
        public string ModelName { get; init; }
        public string BrandImageUrl { get; init; }
        public decimal MinPrice { get; init; }
        public decimal MaxPrice { get; init; }
        public CarColor? CarColor { get; init; }
    }
}