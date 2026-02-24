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
        string LocationDisplayName,
        bool InSale,
        bool IsDeleted
    );
}