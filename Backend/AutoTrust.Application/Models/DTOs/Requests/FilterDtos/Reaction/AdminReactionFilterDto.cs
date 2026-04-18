namespace AutoTrust.Application.Models.DTOs.Requests.FilterDtos.Reaction
{
    public record AdminReactionFilterDto
    (
        bool? IsDeleted = null,
        bool? SortByAsc = false
    ) : ReactionFilterDto;
}
