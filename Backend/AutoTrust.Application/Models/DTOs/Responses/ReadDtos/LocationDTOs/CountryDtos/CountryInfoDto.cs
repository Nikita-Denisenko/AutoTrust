namespace AutoTrust.Application.Models.DTOs.Responses.ReadDtos.LocationDTOs.CountryDtos
{
    public record CountryInfoDto
    (
        int Id,
        string Name,
        string Code,
        string FlagImageUrl,
        string PhoneCode,
        string Currency
    );
}
