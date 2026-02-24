namespace AutoTrust.Application.Models.DTOs.Responses.ReadDtos.CarOwnershipDtos
{
    public record PublicOwnershipListItemDto
    (
        int Id,
        string UserName,
        DateOnly FromDate,
        DateOnly? ToDate,
        string ModelName,
        bool IsCurrent
    );
}