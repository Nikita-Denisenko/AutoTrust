namespace AutoTrust.Application.Models.DTOs.Responses.ReadDtos.BrandDtos
{
    public record PublicBrandListItemDto
    (
        int Id,
        string Name,
        string LogoUrl,
        int CarQuantity
    );
}
