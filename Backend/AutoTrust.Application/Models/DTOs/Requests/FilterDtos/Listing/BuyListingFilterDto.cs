using AutoTrust.Domain.Enums;
using AutoTrust.Domain.Enums.OrderParams;
using static AutoTrust.Domain.Enums.OrderParams.BuyListingOrderParam;

namespace AutoTrust.Application.Models.DTOs.Requests.FilterDtos.Listing
{
    public record BuyListingFilterDto
    (
        BuyListingOrderParam OrderParam = CreatedAt,
        int? ModelId = null,
        decimal? MinPrice = null,
        decimal? MaxPrice = null,
        int? MinReleaseYear = null,
        int? MaxReleaseYear = null,
        CarColor? Color = null
    ) : BaseListingFilterDto;
}
