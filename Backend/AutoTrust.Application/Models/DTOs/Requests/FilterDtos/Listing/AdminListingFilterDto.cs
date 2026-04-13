using AutoTrust.Domain.Enums;

namespace AutoTrust.Application.Models.DTOs.Requests.FilterDtos.Listing
{
    public record AdminListingFilterDto
    (
        bool? IsDeleted = false,
        ListingType? Type = null
    ) : BaseListingFilterDto;
}
