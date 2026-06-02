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
                   new LocationDto
                   {
                       City = new CityDto { Id = src.CityId, CountryId = src.City.CountryId, Name = src.City.Name },
                       Country = new CountryDto
                       {
                           Id = src.City.CountryId,
                           RuName = src.City.Country.RuName,
                           EnName = src.City.Country.EnName,
                           Code = src.City.Country.Code,
                           FlagImageUrl = src.City.Country.FlagImageUrl.Value
                       }
                   }))
               .ForMember(dest => dest.FollowersQuantity, opt => opt.MapFrom(src => src.Followers.Count))
               .ForMember(dest => dest.ReviewsQuantity, opt => opt.MapFrom(src => src.ReceivedReviews.Count))
               .ForMember(dest => dest.FollowingsQuantity, opt => opt.MapFrom(src => src.Followings.Count))
               .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender.ToString()));


            CreateMap<User, AdminUserDto>()
                .ForMember(dest => dest.BirthDate, opt => opt.MapFrom(src => src.BirthDate.Value))
                .ForMember(dest => dest.AvatarUrl, opt => opt.MapFrom(src => src.AvatarUrl == null ? null : src.AvatarUrl.Value))
                .ForMember(dest => dest.Location, opt => opt.MapFrom(src =>
                   new LocationDto
                   {
                       City = new CityDto { Id = src.CityId, CountryId = src.City.CountryId, Name = src.City.Name },
                       Country = new CountryDto
                       {
                           Id = src.City.CountryId,
                           RuName = src.City.Country.RuName,
                           EnName = src.City.Country.EnName,
                           Code = src.City.Country.Code,
                           FlagImageUrl = src.City.Country.FlagImageUrl.Value
                       }
                   }))
                .ForMember(dest => dest.FollowersQuantity, opt => opt.MapFrom(src => src.Followers.Count))
                .ForMember(dest => dest.ReviewsQuantity, opt => opt.MapFrom(src => src.ReceivedReviews.Count))
                .ForMember(dest => dest.FollowingsQuantity, opt => opt.MapFrom(src => src.Followings.Count))
                .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender.ToString()));

            CreateMap<User, AdminUserListItemDto>()
                .ForMember(dest => dest.BirthDate, opt => opt.MapFrom(src => src.BirthDate.Value))
               .ForMember(dest => dest.AvatarUrl, opt => opt.MapFrom(src => src.AvatarUrl == null ? null : src.AvatarUrl.Value))
                .ForMember(dest => dest.Location, opt => opt.MapFrom(src =>
                   new LocationDto
                   {
                       City = new CityDto { Id = src.CityId, CountryId = src.City.CountryId, Name = src.City.Name },
                       Country = new CountryDto
                       {
                           Id = src.City.CountryId,
                           RuName = src.City.Country.RuName,
                           EnName = src.City.Country.EnName,
                           Code = src.City.Country.Code,
                           FlagImageUrl = src.City.Country.FlagImageUrl.Value
                       }
                   }))
                .ForMember(dest => dest.FollowersQuantity, opt => opt.MapFrom(src => src.Followers.Count));


            CreateMap<User, UserProfileListItemDto>()
                .ForMember(dest => dest.BirthDate, opt => opt.MapFrom(src => src.BirthDate.Value))
                .ForMember(dest => dest.AvatarUrl, opt => opt.MapFrom(src => src.AvatarUrl == null ? null : src.AvatarUrl.Value))
               .ForMember(dest => dest.Location, opt => opt.MapFrom(src =>
                   new LocationDto
                   {
                       City = new CityDto { Id = src.CityId, CountryId = src.City.CountryId, Name = src.City.Name },
                       Country = new CountryDto
                       {
                           Id = src.City.CountryId,
                           RuName = src.City.Country.RuName,
                           EnName = src.City.Country.EnName,
                           Code = src.City.Country.Code,
                           FlagImageUrl = src.City.Country.FlagImageUrl.Value
                       }
                   }))
                .ForMember(dest => dest.FollowersQuantity, opt => opt.MapFrom(src => src.Followers.Count));
        }
    }
}
