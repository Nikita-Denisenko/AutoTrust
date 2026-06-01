namespace AutoTrust.Application.Models.DTOs.Responses.ReadDtos.BrandDtos
{
    public record BrandShortDto
    {
        public int Id { get; init; }
        public string Name { get; init; }
        public string LogoUrl { get; init; }
    };
}
