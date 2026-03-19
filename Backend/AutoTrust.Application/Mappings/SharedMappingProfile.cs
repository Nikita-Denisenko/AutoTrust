using AutoMapper;
using AutoTrust.Application.Models.DTOs.Responses.ReadDtos.BrandDtos;
using AutoTrust.Application.Models.DTOs.Responses.ReadDtos.ListingDtos;
using AutoTrust.Application.Models.DTOs.Responses.ReadDtos.LocationDTOs.CityDtos;
using AutoTrust.Application.Models.DTOs.Responses.ReadDtos.LocationDTOs.CountryDtos;
using AutoTrust.Application.Models.DTOs.Responses.ReadDtos.ModelDtos;
using AutoTrust.Application.Models.DTOs.Responses.ReadDtos.UserDtos;
using AutoTrust.Domain.Entities;

namespace AutoTrust.Application.Mappings
{
    public class SharedMappingProfile : Profile
    {
        public SharedMappingProfile() 
        {
            CreateMap<Brand, BrandShortDto>()
                .ForMember(dest => dest.LogoUrl, opt => opt.MapFrom(src => src.LogoUrl.Value));

            CreateMap<Model, ModelShortDto>()
                .IncludeMembers(src => src.Brand);

            CreateMap<Country, CountryShortDto>()
                .ForMember(dest => dest.FlagImageUrl, opt => opt.MapFrom(src => src.FlagImageUrl.Value));

            CreateMap<City, CityDto>();

            CreateMap<User, UserShortDto>()
                .ForMember(dest => dest.AvatarUrl, opt => opt.MapFrom(src => src.AvatarUrl == null ? null : src.AvatarUrl.Value));

            CreateMap<Listing, ListingShortDto>()
                .IncludeMembers(src => src.User);
        }
    }
}
