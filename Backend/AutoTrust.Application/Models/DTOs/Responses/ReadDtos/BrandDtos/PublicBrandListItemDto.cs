namespace AutoTrust.Application.Models.DTOs.Responses.ReadDtos.BrandDtos
{
    public record PublicBrandListItemDto
    {
        public int Id { get; init; }
        public string Name { get; init; }
        public string LogoUrl { get; init; }
        public int CarQuantity { get; init; }
    }
}