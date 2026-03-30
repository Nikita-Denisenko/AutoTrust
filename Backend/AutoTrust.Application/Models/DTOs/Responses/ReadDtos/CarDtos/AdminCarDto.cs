using AutoTrust.Application.Models.DTOs.Responses.ReadDtos.LocationDTOs;
using AutoTrust.Application.Models.DTOs.Responses.ReadDtos.ModelDtos;
using AutoTrust.Domain.Enums;

namespace AutoTrust.Application.Models.DTOs.Responses.ReadDtos.CarDtos
{
    public record AdminCarDto
    (
        int Id,
        string Description,
        int ReleaseYear,
        string ImageUrl,
        CarColor Color,
        string StateNumber,
        decimal EngineMileage,
        int OwnershipsQuantity,
        ModelShortDto Model,
        bool HasAccident,
        bool InSale,
        bool IsDeleted
    );
}