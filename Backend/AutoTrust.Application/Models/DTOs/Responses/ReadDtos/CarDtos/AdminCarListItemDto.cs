using AutoTrust.Application.Models.DTOs.Responses.ReadDtos.LocationDTOs;
using AutoTrust.Application.Models.DTOs.Responses.ReadDtos.ModelDtos;
using AutoTrust.Domain.Enums;

namespace AutoTrust.Application.Models.DTOs.Responses.ReadDtos.CarDtos
{
    public record AdminCarListItemDto
    (
        int Id,
        int OwnerId,
        int ReleaseYear,
        ModelShortDto Model,
        string ImageUrl,
        CarColor Color,
        LocationDto Location,
        bool InSale,
        bool IsDeleted
    );
}