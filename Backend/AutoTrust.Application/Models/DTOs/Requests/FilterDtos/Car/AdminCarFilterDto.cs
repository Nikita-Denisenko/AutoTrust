namespace AutoTrust.Application.Models.DTOs.Requests.FilterDtos.Car
{
    public record AdminCarFilterDto
    (
        bool? IsDeleted = null
    ) : CarFilterDto;
}