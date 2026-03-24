namespace AutoTrust.Application.Models.DTOs.Requests.FilterDtos.Brand
{
    public record AdminBrandFilterDto
    (
        bool? IsActive = null
    ) : BrandFilterDto;
}
