namespace AutoTrust.Application.Models.DTOs.Responses.CreatedDtos
{
    public record CreatedReviewDto
    (
        int Id,
        int Stars,
        int ReviewerId,
        int UserId
    );
}
