using AutoTrust.Domain.Enums.OrderParams;
using static AutoTrust.Domain.Enums.OrderParams.BrandOrderParam;

namespace AutoTrust.Application.Models.DTOs.Requests.FilterDtos.Brand
{
    public record AdminBrandFilterDto : BrandFilterDto
    {
        public bool? IsActive { get; init; } = null;

        public AdminBrandFilterDto(
            int Page = 1,
            int Size = 20,
            string? SearchText = null,
            int? CountryId = null,
            BrandOrderParam OrderParam = Name,
            bool ByAscending = true,
            bool? IsActive = null
        ) : base(Page, Size, SearchText, CountryId, OrderParam, ByAscending)
        {
            this.IsActive = IsActive;
        }
    }
}