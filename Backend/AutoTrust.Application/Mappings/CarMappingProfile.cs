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
                .ForMember(dest => dest.OwnershipsQuantity, opt => opt.MapFrom(src => src.OwnershipHistory.Count));

            CreateMap<Car, AdminCarDto>()
                .IncludeBase<Car, PublicCarDto>()
                .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => src.IsDeleted));

            CreateMap<Car, PublicCarListItemDto>()
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.ImageUrl.Value));

            CreateMap<Car, AdminCarListItemDto>()
                .IncludeBase<Car, PublicCarListItemDto>()
                .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => src.IsDeleted))
                .ForMember(dest => dest.OwnerId, opt => opt.MapFrom(src => src.OwnershipHistory.First(co => co.IsCurrent).UserId));

            CreateMap<CreateCarDto, Car>()
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => Url.Create(src.ImageUrl)))
                .ForMember(dest => dest.StateNumber, opt => opt.MapFrom(src => new StateNumber(src.StateNumber)))
                .ForMember(dest => dest.OwnershipHistory, opt => opt.Ignore())
                .ConstructUsing(src => new Car
                (
                    src.Description,
                    src.ReleaseYear,
                    Url.Create(src.ImageUrl),
                    src.Color,
                    new StateNumber(src.StateNumber),
                    src.ModelId,
                    src.HasAccident,
                    src.UserId,
                    src.FromDate,
                    src.HadMajorRepair,
                    Url.Create(src.BillOfSalePhotoUrl)
                ));

            CreateMap<Car, CreatedCarDto>();
        }
    }
}
