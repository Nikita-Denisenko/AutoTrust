using AutoTrust.Domain.Enums;

namespace AutoTrust.Application.Models.DTOs.Responses.ReadDtos.ListingDtos.SaleListingDtos
{
    public class SaleInfoDto
    {
        public decimal Price { get; set; }
        public int CarId { get; set; }
        public string CarImageUrl { get; set; }
        public string ModelName { get; set; }
        public CarColor? CarColor { get; set; }
        public int ReleaseYear { get; set; } 
        public decimal Mileage { get; set; }
        public int OwnershipsQuantity { get; set; }
        public bool HasAccident {  get; set; }
    }
}