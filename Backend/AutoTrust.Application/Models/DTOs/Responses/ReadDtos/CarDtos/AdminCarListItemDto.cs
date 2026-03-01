using AutoTrust.Application.Models.DTOs.Responses.ReadDtos.LocationDTOs;
using AutoTrust.Domain.Enums;

namespace AutoTrust.Application.Models.DTOs.Responses.ReadDtos.CarDtos
{
    public record AdminCarListItemDto
    (
        int Id,
        int OwnerId,
        int ReleaseYear,
        string ModelName,
        string ImageUrl,
        CarColor Color,
        LocationDto Location,
        bool InSale,
        bool IsDeleted
    );
}