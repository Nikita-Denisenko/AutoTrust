using AutoTrust.Domain.Enums.OrderParams;
using static AutoTrust.Domain.Enums.OrderParams.CarOrderParam;

namespace AutoTrust.Application.Models.DTOs.Requests.FilterDtos.Car
{
    public record CarFilterDto
    (
        int Page = 1,
        int Size = 10,
        string SearchText = "",
        bool? InSale = null,
        bool? HasAccident = null,
        bool? HadMajorRepair = null,
        CarOrderParam OrderParam = ReleaseYear,
        bool ByAscending = false
    );
}
