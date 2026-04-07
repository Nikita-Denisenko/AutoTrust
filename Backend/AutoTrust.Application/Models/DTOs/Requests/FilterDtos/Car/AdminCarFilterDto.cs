namespace AutoTrust.Application.Models.DTOs.Requests.FilterDtos.Car
{
    public record AdminCarFilterDto
    (
        bool? IsDeleted = null,
        int? UserId = null
    ) : CarFilterDto;
}