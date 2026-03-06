using AutoTrust.Application.Models.DTOs.Responses.ReadDtos.UserDtos;
using AutoTrust.Domain.Enums;

namespace AutoTrust.Application.Models.DTOs.Responses.ReadDtos.ListingDtos
{
    public record ListingShortDto
    (
        int Id,
        string Name,
        UserShortDto Author,
        ListingType Type
    );
}
