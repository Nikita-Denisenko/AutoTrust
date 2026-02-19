namespace AutoTrust.Application.Models.DTOs.Responses.CreatedDtos
{
    public record CreatedListingDto
    (
        int Id,
        string Name,
        int UserId,
        int CountryId,
        int CityId
    );
}