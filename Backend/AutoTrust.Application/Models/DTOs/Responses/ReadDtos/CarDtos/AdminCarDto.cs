using AutoTrust.Application.Models.DTOs.Responses.ReadDtos.LocationDTOs;
using AutoTrust.Application.Models.DTOs.Responses.ReadDtos.ModelDtos;
using AutoTrust.Domain.Enums;

namespace AutoTrust.Application.Models.DTOs.Responses.ReadDtos.CarDtos
{
    public record AdminCarDto
    {
        public int Id { get; init; }
        public string Description { get; init; }
        public int ReleaseYear { get; init; }
        public string ImageUrl { get; init; }
        public CarColor Color { get; init; }
        public string StateNumber { get; init; }
        public decimal EngineMileage { get; init; }
        public int OwnershipsQuantity { get; init; }
        public ModelShortDto Model {  get; init; }
        public bool HasAccident {  get; init; }
        public bool InSale { get; init; }
        public bool IsDeleted { get; init; }
    };
}