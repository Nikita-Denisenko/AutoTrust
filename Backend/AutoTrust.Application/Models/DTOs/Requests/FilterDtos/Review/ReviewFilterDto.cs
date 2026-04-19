namespace AutoTrust.Application.Models.DTOs.Requests.FilterDtos.Review
{
    public record ReviewFilterDto
    (
        int Page = 1,
        int Size = 20,
        bool SortByAsc = false,
        int? Stars = null
    );
}
