using AutoTrust.Application.Models.DTOs.Responses.ReadDtos.BrandDtos;

namespace AutoTrust.Application.Models.DTOs.Responses.ReadDtos.ModelDtos
{
    public record AdminModelListItemDto
    (
        int Id,
        string Name,
        string ImageUrl,
        BrandShortDto Brand,
        bool IsActive
    );
}
