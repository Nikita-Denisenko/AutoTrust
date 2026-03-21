using AutoMapper;
using AutoTrust.Application.Models.DTOs.Requests.CreateDtos;
using AutoTrust.Application.Models.DTOs.Responses.CreatedDtos;
using AutoTrust.Application.Models.DTOs.Responses.ReadDtos.CarOwnershipDtos;
using AutoTrust.Domain.Entities;

namespace AutoTrust.Application.Mappings
{
    public class CarOwnershipMappingProfile : Profile
    {
        public CarOwnershipMappingProfile() 
        {
            CreateMap<CarOwnership, PublicCarOwnershipDto>()
                .ForMember(dest => dest.ModelName, opt => opt.MapFrom(src => src.Car.Model.Name))
                .IncludeMembers(src => src.User);

            CreateMap<CarOwnership, PublicUserOwnershipDto>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.Name))
                .ForMember(dest => dest.ModelName, opt => opt.MapFrom(src => src.Car.Model.Name));

            CreateMap<CarOwnership, PublicOwnershipListItemDto>()
                .IncludeBase<CarOwnership, PublicUserOwnershipDto>();

            CreateMap<CarOwnership, AdminOwnershipDto>()
                .IncludeBase<CarOwnership, PublicCarOwnershipDto>()
                .ForMember(dest => dest.CarId, opt => opt.MapFrom(src => src.CarId));

            CreateMap<CarOwnership, AdminOwnershipListItemDto>()
                .IncludeBase<CarOwnership, AdminOwnershipDto>();

            CreateMap<CreateCarOwnershipDto, CarOwnership>();

            CreateMap<CarOwnership, CreatedCarOwnershipDto>();
        }
    }
}
