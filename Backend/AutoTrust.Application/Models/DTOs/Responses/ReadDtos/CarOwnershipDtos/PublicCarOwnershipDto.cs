namespace AutoTrust.Application.Models.DTOs.Responses.ReadDtos.CarOwnershipDtos
{
    public record PublicCarOwnershipDto
    (
        int Id,
        int UserId,
        string UserName,
        decimal MileageBefore,
        decimal? MileageAfter,
        DateOnly FromDate,
        DateOnly? ToDate,
        string ModelName,
        bool IsCurrent
    );
}