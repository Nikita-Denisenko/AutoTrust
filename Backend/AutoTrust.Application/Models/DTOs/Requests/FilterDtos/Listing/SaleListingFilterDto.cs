using AutoTrust.Domain.Enums;
using AutoTrust.Domain.Enums.OrderParams;
using static AutoTrust.Domain.Enums.OrderParams.SaleListingOrderParam;

namespace AutoTrust.Application.Models.DTOs.Requests.FilterDtos.Listing
{
    public record SaleListingFilterDto
    (
        SaleListingOrderParam OrderParam = CreatedAt,
        int? ModelId = null,
        decimal? MinPrice = null,
        decimal? MaxPrice = null,
        int? MinReleaseYear = null,
        int? MaxReleaseYear = null,
        CarColor? Color = null,
        decimal? MinEngineMileage = null,
        decimal? MaxEngineMileage = null
    ) : BaseListingFilterDto;
}
