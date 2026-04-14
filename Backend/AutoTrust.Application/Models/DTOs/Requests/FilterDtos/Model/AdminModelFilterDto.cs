namespace AutoTrust.Application.Models.DTOs.Requests.FilterDtos.Model
{
    public record AdminModelFilterDto
    (
        bool? IsActive = null
    ) : ModelFilterDto;
}
