namespace AutoTrust.Application.Models.DTOs.Responses.CreatedDtos
{
    public record CreatedReactionDto
    (
        int Id,
        string Name,
        int UserId,
        int ListingId
    );
}
