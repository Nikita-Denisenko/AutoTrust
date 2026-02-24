namespace AutoTrust.Application.Models.DTOs.Responses.ReadDtos.BrandDtos
{
    public record AdminBrandDto
    (
        int Id,
        string Name,
        string Description,
        string LogoUrl,
        string CountryName,
        bool IsActive
    );
}
