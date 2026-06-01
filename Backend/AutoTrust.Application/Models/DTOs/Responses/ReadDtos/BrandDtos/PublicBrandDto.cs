namespace AutoTrust.Application.Models.DTOs.Responses.ReadDtos.BrandDtos
{
    public record PublicBrandDto
    {
        public int Id { get; init; }
        public string Name { get; init; }
        public string Description { get; init; }
        public string LogoUrl { get; init; }
        public int CarQuantity { get; init; }
        public string CountryName { get; init; }
    };
}
