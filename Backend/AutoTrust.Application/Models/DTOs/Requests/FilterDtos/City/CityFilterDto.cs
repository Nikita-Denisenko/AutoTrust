namespace AutoTrust.Application.Models.DTOs.Requests.FilterDtos.City
{
    public record CityFilterDto
    (
        int Page = 1,
        int Size = 20,
        string SearchText = "",
        int? CountryId = null,
        bool SortByAsc = true
    );
}
