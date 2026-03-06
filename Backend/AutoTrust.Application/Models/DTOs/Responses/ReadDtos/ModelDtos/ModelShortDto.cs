using AutoTrust.Application.Models.DTOs.Responses.ReadDtos.BrandDtos;

namespace AutoTrust.Application.Models.DTOs.Responses.ReadDtos.ModelDtos
{
    public record ModelShortDto
    (
        int Id,
        string Name,
        BrandShortDto Brand
    );
}
