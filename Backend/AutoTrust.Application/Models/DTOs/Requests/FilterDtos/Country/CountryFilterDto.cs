namespace AutoTrust.Application.Models.DTOs.Requests.FilterDtos.Country
{
    public record CountryFilterDto
    (
        int Page = 1,
        int Size = 20,
        string SearchText = "",
        bool SortByAsc = true
    );
}
