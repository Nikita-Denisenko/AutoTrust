using AutoTrust.Application.Models.DTOs.Requests.FilterDtos.User;

namespace AutoTrust.Application.Models.DTOs.Requests.FilterDtos.User
{
    public record UserFilterDto
    (
        int Page = 1,
        int Size = 20,
        int? CityId = null,
        string? SearchText = null,
        bool SortByAsc = true
    );
}