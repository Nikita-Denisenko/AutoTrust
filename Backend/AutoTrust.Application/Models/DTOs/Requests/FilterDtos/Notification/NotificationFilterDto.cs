namespace AutoTrust.Application.Models.DTOs.Requests.FilterDtos.Notification
{
    public record NotificationFilterDto
    (
        int Page = 1,
        int Size = 20,
        bool? IsRead = null,
        bool SortByAsc = false
    );
}
