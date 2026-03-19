namespace AutoTrust.Application.Models.DTOs.Responses.ReadDtos.LocationDTOs.CountryDtos
{
    public record CountryShortDto
    (
        int Id,
        string RuName,
        string EnName,
        string FlagImageUrl
    );
}
