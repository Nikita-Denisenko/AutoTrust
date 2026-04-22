using AutoMapper;
using AutoTrust.Application.Models.DTOs.Responses.ReadDtos.LocationDTOs;
using AutoTrust.Application.Models.DTOs.Responses.ReadDtos.LocationDTOs.CityDtos;
using AutoTrust.Application.Models.DTOs.Responses.ReadDtos.LocationDTOs.CountryDtos;
using AutoTrust.Application.Models.DTOs.Responses.ReadDtos.UserDtos;
using AutoTrust.Domain.Entities;

namespace AutoTrust.Application.Mappings
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            CreateMap<User, UserProfileDto>()
               .ForMember(dest => dest.BirthDate, opt => opt.MapFrom(src => src.BirthDate.Value))
               .ForMember(dest => dest.AvatarUrl, opt => opt.MapFrom(src => src.AvatarUrl == null ? null : src.AvatarUrl.Value))
                .ForMember(dest => dest.Location, opt => opt.MapFrom(src =>
                   new LocationDto(new CityDto(src.CityId, src.City.CountryId, src.City.Name),
                   new CountryDto
                   (
                       src.City.CountryId,
                       src.City.Country.RuName,
                       src.City.Country.EnName,
                       src.City.Country.Code,
                       src.City.Country.FlagImageUrl.Value
                    ))));

            CreateMap<User, AdminUserDto>()
                .IncludeBase<User, UserProfileDto>();
                
            CreateMap<User, AdminUserListItemDto>()
                .IncludeBase<User, UserProfileDto>();

            CreateMap<User, UserProfileListItemDto>()
                .IncludeBase<User, UserProfileDto>();
        }
    }
}
