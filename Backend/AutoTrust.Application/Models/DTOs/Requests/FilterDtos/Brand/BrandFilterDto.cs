using AutoTrust.Domain.Enums.OrderParams;
using static AutoTrust.Domain.Enums.OrderParams.BrandOrderParam;

namespace AutoTrust.Application.Models.DTOs.Requests.FilterDtos.Brand
{
    public record BrandFilterDto
    (
        int Page = 1,
        int Size = 20,
        string SearchText = "",
        int? CountryId = null,
        BrandOrderParam OrderParam = Name,
        bool ByAscending = true
    );
}
