namespace AutoTrust.Application.Models.DTOs.Responses.CreatedDtos
{
    public record CreatedBrandDto
    (
        int Id,
        string Name,
        int CountryId
    );
}