using AutoTrust.Domain.Enums;

namespace AutoTrust.Application.Models.DTOs.Requests.FilterDtos.Listing
{
    public abstract record BaseListingFilterDto
    (
        int Page = 1,
        int Size = 20,
        string SearchText = "",
        int? CityId = null,
        bool SortByAsc = false
    );
}
