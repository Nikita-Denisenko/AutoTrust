namespace AutoTrust.Application.Models.DTOs.Requests.FilterDtos.Follow
{
    public record FollowFilterDto
    (
        int Page = 1,
        int Size = 20,
        string SearchText = ""
    );
}
