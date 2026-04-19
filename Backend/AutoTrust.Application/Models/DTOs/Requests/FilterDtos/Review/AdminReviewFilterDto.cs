namespace AutoTrust.Application.Models.DTOs.Requests.FilterDtos.Review
{
    public record AdminReviewFilterDto
    (
        bool? IsDeleted = null
    ) : ReviewFilterDto;
}
