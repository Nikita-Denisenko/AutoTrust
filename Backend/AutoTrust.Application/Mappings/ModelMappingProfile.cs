using AutoMapper;
using AutoTrust.Application.Models.DTOs.Requests.CreateDtos;
using AutoTrust.Application.Models.DTOs.Responses.CreatedDtos;
using AutoTrust.Application.Models.DTOs.Responses.ReadDtos.ModelDtos;
using AutoTrust.Domain.Entities;
using AutoTrust.Domain.ValueObjects;

namespace AutoTrust.Application.Mappings
{
    public class ModelMappingProfile : Profile
    {
        public ModelMappingProfile() 
        {
            CreateMap<Model, AdminModelDto>()
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.ImageUrl.Value));

            CreateMap<Model, AdminModelListItemDto>()
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.ImageUrl.Value));

            CreateMap<Model, ModelDto>()
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.ImageUrl.Value));

            CreateMap<Model, ModelListItemDto>()
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.ImageUrl.Value));

            CreateMap<CreateModelDto, Model>()
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => Url.Create(src.ImageUrl)));

            CreateMap<Model, CreatedModelDto>();
        }
    }
}
