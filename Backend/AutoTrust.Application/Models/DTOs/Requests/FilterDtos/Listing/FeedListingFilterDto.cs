using AutoTrust.Domain.Enums;

namespace AutoTrust.Application.Models.DTOs.Requests.FilterDtos.Listing
{
    public record FeedListingFilterDto
    (
        ListingType? Type = null
    ) : BaseListingFilterDto;
}
