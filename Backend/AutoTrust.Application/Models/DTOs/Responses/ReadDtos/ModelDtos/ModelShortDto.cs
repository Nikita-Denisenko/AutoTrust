using AutoTrust.Application.Models.DTOs.Responses.ReadDtos.BrandDtos;

namespace AutoTrust.Application.Models.DTOs.Responses.ReadDtos.ModelDtos
{
    public record ModelShortDto
    {
        public int Id { get; init; }
        public string Name { get; init; }
        public BrandShortDto Brand { get; init; }
    };
}
