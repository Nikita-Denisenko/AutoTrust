using AutoTrust.Application.Models.DTOs.Responses.ReadDtos.LocationDTOs;
using AutoTrust.Application.Models.DTOs.Responses.ReadDtos.ModelDtos;
using AutoTrust.Domain.Enums;

namespace AutoTrust.Application.Models.DTOs.Responses.ReadDtos.CarDtos
{
    public class PublicCarDto
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public int ReleaseYear { get; set; }
        public string ImageUrl { get; set; }
        public CarColor Color { get; set; }
        public string StateNumber { get; set; }
        public decimal EngineMileage { get; set; }
        public int OwnershipsQuantity { get; set; }
        public ModelShortDto Model { get; set; }
        public bool HasAccident { get; set; }
        public bool InSale { get; set; }
    }
}