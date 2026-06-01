using AutoTrust.Application.Models.DTOs.Responses.ReadDtos.LocationDTOs;
using AutoTrust.Application.Models.DTOs.Responses.ReadDtos.ModelDtos;
using AutoTrust.Domain.Enums;

namespace AutoTrust.Application.Models.DTOs.Responses.ReadDtos.CarDtos
{
    public record AdminCarListItemDto
    {
        public int Id { get; init; }
        public int OwnerId { get; init; }
        public int ReleaseYear { get; init; }
        public ModelShortDto Model { get; init; }
        public string ImageUrl { get; init; }
        public CarColor Color { get; init; }
        public bool InSale { get; init; }
        public bool IsDeleted { get; init; }
    };
}