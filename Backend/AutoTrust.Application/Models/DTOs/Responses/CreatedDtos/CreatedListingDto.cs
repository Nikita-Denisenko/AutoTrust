using AutoTrust.Domain.Enums;

namespace AutoTrust.Application.Models.DTOs.Responses.CreatedDtos
{
    public record CreatedListingDto
    (
        int Id,
        string Name,
        ListingType Type
    );
}