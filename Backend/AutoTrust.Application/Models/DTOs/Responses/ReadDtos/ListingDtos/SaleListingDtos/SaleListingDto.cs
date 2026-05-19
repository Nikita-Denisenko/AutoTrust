using AutoTrust.Application.Models.DTOs.Responses.ReadDtos.CarDtos;
using AutoTrust.Application.Models.DTOs.Responses.ReadDtos.LocationDTOs;
using AutoTrust.Application.Models.DTOs.Responses.ReadDtos.UserDtos;

namespace AutoTrust.Application.Models.DTOs.Responses.ReadDtos.ListingDtos.SaleListingDtos
{
    public class SaleListingDto
    {
        public int Id { get; set; }
        public UserShortDto Author { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public LocationDto Location { get; set; }
        public decimal Price { get; set; }
        public PublicCarDto Car { get; set; }
        public string Description { get; set; }
        public int ReactionsQuantity { get; set; }
        public bool IsActive { get; set; }
    }
}