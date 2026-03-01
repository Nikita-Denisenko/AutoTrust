using AutoTrust.Application.Models.DTOs.Responses.ReadDtos.LocationDTOs.CityDtos;
using AutoTrust.Application.Models.DTOs.Responses.ReadDtos.LocationDTOs.CountryDtos;

namespace AutoTrust.Application.Models.DTOs.Responses.ReadDtos.LocationDTOs
{
    public record LocationDto
    (
        CityDto City,
        CountryShortDto Country
    );
}
