using AutoTrust.Application.Models.DTOs.Responses.ReadDtos.BrandDtos;

namespace AutoTrust.Application.Models.DTOs.Responses.ReadDtos.ModelDtos
{
    public record AdminModelDto
    (
        int Id,
        string Name,
        string Description,
        string ModelImageUrl,
        BrandShortDto Brand,
        bool IsActive
    );
}
