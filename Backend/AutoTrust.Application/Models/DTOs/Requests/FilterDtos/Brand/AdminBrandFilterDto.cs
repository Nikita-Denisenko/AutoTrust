namespace AutoTrust.Application.Models.DTOs.Requests.FilterDtos.Brand
{
    public record AdminBrandFilterDto
    (
        bool? IsDeleted = null
    ) : BrandFilterDto;
}
