using AutoTrust.Application.Models.DTOs.Responses.ReadDtos.BrandDtos;

namespace AutoTrust.Application.Models.DTOs.Responses.ReadDtos.ModelDtos
{
    public record ModelDto
    {
        public int Id { get; init; }
        public string Name { get; init; }
        public string Description { get; init; }
        public BrandShortDto Brand { get; init; }
    };
}
