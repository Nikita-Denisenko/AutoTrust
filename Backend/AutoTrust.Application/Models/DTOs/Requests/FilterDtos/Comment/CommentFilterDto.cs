namespace AutoTrust.Application.Models.DTOs.Requests.FilterDtos.Comment
{
    public record CommentFilterDto
    (
        int Page = 1,
        int Size = 20,
        bool SortByAsc = false
    );
}
