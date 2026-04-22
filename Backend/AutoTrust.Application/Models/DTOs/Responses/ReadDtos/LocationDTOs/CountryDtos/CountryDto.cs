namespace AutoTrust.Application.Models.DTOs.Responses.ReadDtos.LocationDTOs.CountryDtos
{
    public record CountryDto
    (
        int Id,
        string RuName,
        string EnName,
        string Code,
        string FlagImageUrl
    );
}
