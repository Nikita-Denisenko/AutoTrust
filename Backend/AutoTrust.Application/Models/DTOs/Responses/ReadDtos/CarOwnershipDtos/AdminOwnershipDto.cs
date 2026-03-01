using AutoTrust.Application.Models.DTOs.Responses.ReadDtos.UserDtos;

namespace AutoTrust.Application.Models.DTOs.Responses.ReadDtos.CarOwnershipDtos
{
    public record AdminOwnershipDto
    (
        int Id,
        UserShortDto User,
        decimal MileageBefore,
        decimal? MileageAfter,
        DateOnly FromDate,
        DateOnly? ToDate,
        string ModelName,
        bool IsCurrent,
        int CarId
    );
}