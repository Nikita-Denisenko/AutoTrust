namespace AutoTrust.Application.Models.DTOs.Responses.ReadDtos.BrandDtos
{
    public record AdminBrandListItemDto
    (
        int Id,
        string Name,
        string LogoUrl,
        bool IsActive
    );
}
