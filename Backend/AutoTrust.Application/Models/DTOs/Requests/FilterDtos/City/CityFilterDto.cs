namespace AutoTrust.Application.Models.DTOs.Requests.FilterDtos.City
{
    public record CityFilterDto
    (
        int Page = 1,
        int Size = 1000,
        string SearchText = "",
        int? CountryId = null,
        bool SortByAsc = true
    );
}
