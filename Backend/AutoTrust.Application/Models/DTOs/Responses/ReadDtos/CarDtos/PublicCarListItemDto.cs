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
        string LocationDisplayName,
        bool InSale
    );
}