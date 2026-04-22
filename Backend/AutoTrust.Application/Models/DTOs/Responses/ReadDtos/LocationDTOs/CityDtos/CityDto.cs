namespace AutoTrust.Application.Models.DTOs.Responses.ReadDtos.LocationDTOs.CityDtos
{
    public record CityDto
    (
        int Id,
        int CountryId,
        string Name
    );
}
