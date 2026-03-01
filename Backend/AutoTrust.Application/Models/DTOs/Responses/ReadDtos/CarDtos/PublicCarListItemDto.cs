using AutoTrust.Application.Models.DTOs.Responses.ReadDtos.LocationDTOs;
using AutoTrust.Domain.Enums;

namespace AutoTrust.Application.Models.DTOs.Responses.ReadDtos.CarDtos
{
    public record PublicCarListItemDto
    (
        int Id,
        int ReleaseYear,
        string ModelName,
        string ImageUrl,
        CarColor Color,
        LocationDto Location,
        bool InSale
    );
}