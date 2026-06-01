namespace AutoTrust.Application.Models.DTOs.Responses.ReadDtos.LocationDTOs.CityDtos
{
    public record CityDto
    {
        public int Id { get; init; }
        public int CountryId { get; init; }
        public string Name { get; init; }
    };
}
