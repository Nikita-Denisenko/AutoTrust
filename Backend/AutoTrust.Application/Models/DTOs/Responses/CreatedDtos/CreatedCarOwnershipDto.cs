namespace AutoTrust.Application.Models.DTOs.Responses.CreatedDtos
{
    public record CreatedCarOwnershipDto
    (
        int Id,
        int UserId,
        int CarId
    );
}