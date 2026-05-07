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
            CreateMap<Model, AdminModelDto>();
            CreateMap<Model, AdminModelListItemDto>();
            CreateMap<Model, ModelDto>();
            CreateMap<Model, ModelListItemDto>();
            CreateMap<CreateModelDto, Model>();
            CreateMap<Model, CreatedModelDto>();
        }
    }
}
