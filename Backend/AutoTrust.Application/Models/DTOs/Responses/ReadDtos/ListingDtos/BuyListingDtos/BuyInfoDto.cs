using AutoTrust.Domain.Enums;

namespace AutoTrust.Application.Models.DTOs.Responses.ReadDtos.ListingDtos.BuyListingDtos
{
    public class BuyInfoDto
    {
        public int ModelId { get; set; }
        public string ModelName { get; set; }
        public string BrandImageUrl { get; set; }
        public decimal MinPrice { get; set; }
        public decimal MaxPrice { get; set; }
        public CarColor? CarColor { get; set; }
    }
}