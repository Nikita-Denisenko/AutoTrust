namespace AutoTrust.Application.Models.DTOs.Responses.CreatedDtos
{
    public record CreatedFollowDto
    (
        int Id,
        int FollowerId,
        int TargetId
    );
}
