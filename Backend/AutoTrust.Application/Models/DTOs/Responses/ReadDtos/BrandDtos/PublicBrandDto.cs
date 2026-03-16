namespace AutoTrust.Application.Models.DTOs.Responses.ReadDtos.BrandDtos
{
    public record PublicBrandDto
    (
        int Id,
        string Name,
        string Description,
        string LogoUrl,
        int CarQuantity,
        string CountryName
    );
}
