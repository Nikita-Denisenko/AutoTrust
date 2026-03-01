using AutoTrust.Application.Models.DTOs.Responses.ReadDtos.UserDtos;

namespace AutoTrust.Application.Models.DTOs.Responses.ReadDtos.CarOwnershipDtos
{
    public record AdminOwnershipListItemDto
    (
         int Id,
         UserShortDto User,
         DateOnly FromDate,
         DateOnly? ToDate,
         string ModelName,
         bool IsCurrent,
         int CarId
    );
}
