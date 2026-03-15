using AutoTrust.Domain.Enums.OrderParams;
using static AutoTrust.Domain.Enums.OrderParams.AdminAccountOrderParam;


namespace AutoTrust.Application.Models.DTOs.Requests.FilterDtos.Account
{
    public record AdminAccountFilterDto
    (
        int Page = 1,
        int Size = 20,
        string SearchText = "",
        bool? IsDeleted = null,
        AdminAccountOrderParam OrderParam = Email,
        bool ByAscending = true
    );
}
