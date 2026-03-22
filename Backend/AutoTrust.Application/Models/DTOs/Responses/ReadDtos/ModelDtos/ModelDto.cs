using AutoTrust.Application.Models.DTOs.Responses.ReadDtos.BrandDtos;

namespace AutoTrust.Application.Models.DTOs.Responses.ReadDtos.ModelDtos
{
    public record ModelDto
    (
        int Id,
        string Name,
        string Description,
        string ImageUrl,
        BrandShortDto Brand
    );
}
