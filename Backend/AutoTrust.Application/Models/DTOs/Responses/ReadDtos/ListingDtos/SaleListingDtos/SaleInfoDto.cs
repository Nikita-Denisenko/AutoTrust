using AutoTrust.Domain.Enums;

namespace AutoTrust.Application.Models.DTOs.Responses.ReadDtos.ListingDtos.SaleListingDtos
{
    public record SaleInfoDto
    {
        public decimal Price { get; init; }
        public int CarId { get; init; }
        public string CarImageUrl { get; init; }
        public string ModelName { get; init; }
        public CarColor? CarColor { get; init; }
        public int ReleaseYear { get; init; } 
        public decimal Mileage { get; init; }
        public int OwnershipsQuantity { get; init; }
        public bool HasAccident {  get; init; }
    }
}