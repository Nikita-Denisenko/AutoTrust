using AutoTrust.Domain.Enums;

namespace AutoTrust.Application.Models.DTOs.Responses.ReadDtos.CarDtos
{
    public record PublicCarDto
    (
        int Id,
        string Description,
        int ReleaseYear,
        string ImageUrl,
        CarColor Color,
        string StateNumber,
        decimal EngineMileage,
        int OwnershipsQuantity,
        int ModelId,
        string ModelName,
        string LocationDisplayName,
        bool HasAccident,
        bool InSale
    );
}