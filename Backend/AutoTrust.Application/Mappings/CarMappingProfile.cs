using AutoMapper;
using AutoTrust.Application.Models.DTOs.Requests.CreateDtos;
using AutoTrust.Application.Models.DTOs.Responses.CreatedDtos;
using AutoTrust.Application.Models.DTOs.Responses.ReadDtos.CarDtos;
using AutoTrust.Application.Models.DTOs.Responses.ReadDtos.LocationDTOs;
using AutoTrust.Application.Models.DTOs.Responses.ReadDtos.LocationDTOs.CityDtos;
using AutoTrust.Application.Models.DTOs.Responses.ReadDtos.LocationDTOs.CountryDtos;
using AutoTrust.Domain.Entities;
using AutoTrust.Domain.ValueObjects;

namespace AutoTrust.Application.Mappings
{
    public class CarMappingProfile : Profile
    {
        public CarMappingProfile() 
        {
            CreateMap<Car, PublicCarDto>()
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.ImageUrl.Value))
                .ForMember(dest => dest.StateNumber, opt => opt.MapFrom(src => src.StateNumber.Value))
                .ForMember(dest => dest.OwnershipsQuantity, opt => opt.MapFrom(src => src.OwnershipHistory.Count))
                .ForMember(dest => dest.Location, opt => opt.MapFrom(src => 
                    new LocationDto(new CityDto(src.LocationCityId, src.LocationCity.Name), 
                    new CountryShortDto
                    (
                        src.LocationCity.CountryId, 
                        src.LocationCity.Country.RuName, 
                        src.LocationCity.Country.EnName, 
                        src.LocationCity.Country.FlagImageUrl.Value
                     ))))
                .IncludeMembers(src => src.Model);

            CreateMap<Car, AdminCarDto>()
                .IncludeBase<Car, PublicCarDto>()
                .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => src.IsDeleted));

            CreateMap<Car, PublicCarListItemDto>()
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.ImageUrl.Value))
                .ForMember(dest => dest.Location, opt => opt.MapFrom(src => 
                    new LocationDto(new CityDto(src.LocationCityId, src.LocationCity.Name), 
                    new CountryShortDto
                    (
                        src.LocationCity.CountryId, 
                        src.LocationCity.Country.RuName, 
                        src.LocationCity.Country.EnName, 
                        src.LocationCity.Country.FlagImageUrl.Value
                     ))))
                .IncludeMembers(src => src.Model);

            CreateMap<Car, AdminCarListItemDto>()
                .IncludeBase<Car, PublicCarListItemDto>()
                .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => src.IsDeleted))
                .ForMember(dest => dest.OwnerId, opt => opt.MapFrom(src => src.OwnershipHistory.First(co => co.IsCurrent).UserId));

            CreateMap<CreateCarDto, Car>()
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => Url.Create(src.ImageUrl)))
                .ForMember(dest => dest.StateNumber, opt => opt.MapFrom(src => new StateNumber(src.StateNumber)));

            CreateMap<Car, CreatedCarDto>();
        }
    }
}
