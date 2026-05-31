using AutoTrust.Application.Models.DTOs.Responses.ReadDtos.LocationDTOs;
using AutoTrust.Application.Models.DTOs.Responses.ReadDtos.ModelDtos;
using AutoTrust.Domain.Enums;

namespace AutoTrust.Application.Models.DTOs.Responses.ReadDtos.CarDtos
{
    public record class PublicCarListItemDto
    {
        public int Id { get; set; }
        public int ReleaseYear { get; set; }
        public ModelShortDto Model { get; set; }
        public string ImageUrl { get; set; }
        public decimal EngineMileage { get; set; }
        public int OwnershipsQuantity { get; set; }
        public CarColor Color { get; set; }
        public bool HasAccident { get; set; }
        public bool InSale { get; set; }
    }
}