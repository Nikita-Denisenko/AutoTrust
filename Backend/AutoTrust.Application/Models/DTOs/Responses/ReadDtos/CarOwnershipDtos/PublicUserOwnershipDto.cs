namespace AutoTrust.Application.Models.DTOs.Responses.ReadDtos.CarOwnershipDtos
{
    public record PublicUserOwnershipDto
    (
        int Id,
        string UserName,
        decimal MileageBefore,
        decimal? MileageAfter,
        DateOnly FromDate,
        DateOnly? ToDate,
        string ModelName,
        int CarId,
        bool IsCurrent
    );
}