using AutoTrust.Application.Models.DTOs.Responses.ReadDtos.UserDtos;

namespace AutoTrust.Application.Models.DTOs.Responses.ReadDtos.CarOwnershipDtos
{
    public record AdminOwnershipDto
    {
        public int Id { get; init; }
        public UserShortDto User { get; init; }
        public decimal MileageBefore { get; init; }
        public decimal? MileageAfter { get; init; }
        public DateOnly FromDate { get; init; }
        public DateOnly? ToDate { get; init; }
        public string ModelName { get; init; }
        public bool IsCurrent { get; init; }
        public int CarId { get; init; }
    };
}