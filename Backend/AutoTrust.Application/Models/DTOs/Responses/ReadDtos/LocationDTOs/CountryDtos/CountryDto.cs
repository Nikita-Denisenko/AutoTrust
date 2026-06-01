namespace AutoTrust.Application.Models.DTOs.Responses.ReadDtos.LocationDTOs.CountryDtos
{
    public record CountryDto
    {
        public int Id { get; init; }
        public string RuName { get; init; }
        public string EnName { get; init; }
        public string Code { get; init; }
        public string FlagImageUrl { get; init; }
    };
}
