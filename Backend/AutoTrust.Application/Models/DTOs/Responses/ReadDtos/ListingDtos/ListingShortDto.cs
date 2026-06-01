using AutoTrust.Application.Models.DTOs.Responses.ReadDtos.UserDtos;
using AutoTrust.Domain.Enums;

namespace AutoTrust.Application.Models.DTOs.Responses.ReadDtos.ListingDtos
{
    public record ListingShortDto
    {
        public int Id { get; init; }
        public string Name { get; init; }
        public UserShortDto Author { get; init; }
        public ListingType Type { get; init; }
    };
}
