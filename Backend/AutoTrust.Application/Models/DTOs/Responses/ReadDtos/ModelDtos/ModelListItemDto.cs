using AutoTrust.Application.Models.DTOs.Responses.ReadDtos.BrandDtos;

namespace AutoTrust.Application.Models.DTOs.Responses.ReadDtos.ModelDtos
{
    public record ModelListItemDto
    (
        int Id,
        string Name,
        string ModelImageUrl,
        BrandShortDto Brand
    );
}
