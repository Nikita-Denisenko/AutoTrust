using AutoMapper;
using AutoTrust.Application.Models.DTOs.Responses.ReadDtos.LocationDTOs.CityDtos;
using AutoTrust.Application.Models.DTOs.Responses.ReadDtos.LocationDTOs.CountryDtos;
using AutoTrust.Domain.Entities;
namespace AutoTrust.Application.Mappings
{
    public class LocationMappingProfile : Profile
    {
        public LocationMappingProfile() 
        {
            CreateMap<City, CityDto>();
            CreateMap<Country, CountryDto>();
        }
    }
}
