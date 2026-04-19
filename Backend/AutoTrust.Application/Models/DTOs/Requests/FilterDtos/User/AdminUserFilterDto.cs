namespace AutoTrust.Application.Models.DTOs.Requests.FilterDtos.User
{
    public record AdminUserFilterDto
    (
        bool? IsDeleted = null,
        bool? IsBlocked = null
    ) : UserFilterDto;
}
