using AutoMapper;
using AutoTrust.Application.Models.DTOs.Requests.CreateDtos;
using AutoTrust.Application.Models.DTOs.Responses.CreatedDtos;
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
                .ForMember(dest => dest.CarQuantity, opt => opt.MapFrom(src => src.Models.SelectMany(m => m.Cars).Count()))
                .ForMember(dest => dest.CountryName, opt => opt.MapFrom(src => src.Country.RuName));

            CreateMap<Brand, AdminBrandDto>()
                .IncludeBase<Brand, PublicBrandDto>();

            CreateMap<Brand, PublicBrandListItemDto>()
                .ForMember(dest => dest.CarQuantity, opt => opt.MapFrom(src => src.Models.SelectMany(m => m.Cars).Count()))
                .ForMember(dest => dest.LogoUrl, opt => opt.MapFrom(src => src.LogoUrl.Value));

            CreateMap<Brand, AdminBrandListItemDto>()
                .IncludeBase<Brand, PublicBrandListItemDto>();

            CreateMap<Brand, CreatedBrandDto>();

            CreateMap<CreateBrandDto, Brand>()
                .ForMember(dest => dest.LogoUrl, opt => opt.MapFrom(src => Url.Create(src.LogoUrl)));
        }
    }
}
