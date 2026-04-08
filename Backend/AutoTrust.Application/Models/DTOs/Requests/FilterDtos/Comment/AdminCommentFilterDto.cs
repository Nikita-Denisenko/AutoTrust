namespace AutoTrust.Application.Models.DTOs.Requests.FilterDtos.Comment
{
    public record AdminCommentFilterDto
    (
        int? UserId = null,
        int? ListingId = null
    ) : CommentFilterDto;
}
