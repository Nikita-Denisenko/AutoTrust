namespace AutoTrust.Application.Models.DTOs.Responses.ReadDtos.CarOwnershipDtos
{
    public record PublicOwnershipListItemDto
    {
        public int Id { get; init; }
        public string UserName { get; init; }
        public DateOnly FromDate { get; init; }
        public DateOnly? ToDate { get; init; }
        public string ModelName { get; init; }
        public bool IsCurrent {  get; init; }
    };
}