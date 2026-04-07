namespace AutoTrust.Application.Models.DTOs.Requests.FilterDtos.Message
{
    public record AdminMessageFilterDto
    (
        bool? IsDeleted = null,
        bool SortByAsc = false,
        int? UserId = null,
        int? ChatId = null
    ) : MessageFilterDto;
}
