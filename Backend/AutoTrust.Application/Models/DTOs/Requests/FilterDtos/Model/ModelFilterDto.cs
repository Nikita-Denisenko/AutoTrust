using Microsoft.EntityFrameworkCore.Query;

namespace AutoTrust.Application.Models.DTOs.Requests.FilterDtos.Model
{
    public record ModelFilterDto
    (
        int Page = 1,
        int Size = 20,
        int? BrandId = null,
        string SearchText = "",
        bool SortByAsc = true
    );
}
