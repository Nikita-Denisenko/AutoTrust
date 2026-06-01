namespace AutoTrust.Application.Models.DTOs.Responses.ReadDtos.CarOwnershipDtos
{
    public record PublicUserOwnershipDto
    {
        public int Id { get; init; }
        public string UserName { get; init; }
        public decimal MileageBefore { get; init; }
        public decimal? MileageAfter { get; init; }
        public DateOnly FromDate { get; init; }
        public DateOnly? ToDate { get; init; }
        public string ModelName { get; init; }
        public int CarId { get; init; }
        public bool IsCurrent { get; init; }
    };
}