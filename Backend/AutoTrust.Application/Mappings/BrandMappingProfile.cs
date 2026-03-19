using AutoMapper;
using AutoTrust.Application.Models.DTOs.Requests.CreateDtos;
using AutoTrust.Application.Models.DTOs.Responses.ReadDtos.BrandDtos;
using AutoTrust.Domain.Entities;
using AutoTrust.Domain.ValueObjects;

namespace AutoTrust.Application.Mappings
{
    public class BrandMappingProfile : Profile
    {
        public BrandMappingProfile() 
        {
            CreateMap<Brand, PublicBrandDto>()
                .ForMember(dest => dest.CarQuantity, opt => opt.Ignore())
                .ForMember(dest => dest.CountryName, opt => opt.MapFrom(src => src.Country.RuName));

            CreateMap<Brand, AdminBrandDto>()
                .IncludeBase<Brand, PublicBrandDto>()
                .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => src.IsDeleted));

            CreateMap<Brand, PublicBrandListItemDto>()
                .ForMember(dest => dest.CarQuantity, opt => opt.Ignore())
                .ForMember(dest => dest.LogoUrl, opt => opt.MapFrom(src => src.LogoUrl.Value));

            CreateMap<Brand, AdminBrandListItemDto>()
                .IncludeBase<Brand, PublicBrandListItemDto>()
                .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => src.IsDeleted));

            CreateMap<CreateBrandDto, Brand>()
                .ForMember(dest => dest.LogoUrl, opt => opt.MapFrom(src => Url.Create(src.LogoUrl)));
        }
    }
}
