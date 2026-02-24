namespace AutoTrust.Application.Models.DTOs.Responses.ReadDtos.CarOwnershipDtos
{
    public record AdminOwnershipListItemDto
    (
         int Id,
         int UserId,
         string UserName,
         DateOnly FromDate,
         DateOnly? ToDate,
         string ModelName,
         bool IsCurrent,
         int CarId
    );
}
