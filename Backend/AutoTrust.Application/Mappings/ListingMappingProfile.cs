using AutoMapper;
using AutoTrust.Application.Models.DTOs.Requests.CreateDtos;
using AutoTrust.Application.Models.DTOs.Responses.CreatedDtos;
using AutoTrust.Application.Models.DTOs.Responses.ReadDtos.BrandDtos;
using AutoTrust.Application.Models.DTOs.Responses.ReadDtos.CarDtos;
using AutoTrust.Application.Models.DTOs.Responses.ReadDtos.ListingDtos;
using AutoTrust.Application.Models.DTOs.Responses.ReadDtos.ListingDtos.BuyListingDtos;
using AutoTrust.Application.Models.DTOs.Responses.ReadDtos.ListingDtos.SaleListingDtos;
using AutoTrust.Application.Models.DTOs.Responses.ReadDtos.LocationDTOs;
using AutoTrust.Application.Models.DTOs.Responses.ReadDtos.LocationDTOs.CityDtos;
using AutoTrust.Application.Models.DTOs.Responses.ReadDtos.LocationDTOs.CountryDtos;
using AutoTrust.Application.Models.DTOs.Responses.ReadDtos.ModelDtos;
using AutoTrust.Domain.Entities;
using AutoTrust.Domain.Enums;
using AutoTrust.Domain.ValueObjects;

namespace AutoTrust.Application.Mappings
{
    public class ListingMappingProfile : Profile
    {
        public ListingMappingProfile()
        {
            CreateMap<BuyDetails, BuyInfoDto>()
                .ForMember(dest => dest.ModelName, opt => opt.MapFrom(src => src.Model != null ? src.Model.Name : null))
                .ForMember(dest => dest.BrandImageUrl, opt => opt.MapFrom(src => src.Model != null && src.Model.Brand != null ? src.Model.Brand.LogoUrl : null))
                .ForMember(dest => dest.CarColor, opt => opt.MapFrom(src => src.CarColor));

            CreateMap<SaleDetails, SaleInfoDto>()
    .ForMember(dest => dest.ModelName, opt => opt.MapFrom(src => src.Car != null && src.Car.Model != null ? src.Car.Model.Name : null))
    .ForMember(dest => dest.CarImageUrl, opt => opt.MapFrom(src => src.Car != null && src.Car.ImageUrl != null ? src.Car.ImageUrl.Value : null))
     .ForMember(dest => dest.ReleaseYear, opt => opt.MapFrom(src => src.Car.ReleaseYear))
    .ForMember(dest => dest.Mileage, opt => opt.MapFrom(src => src.Car.EngineMileage))
    .ForMember(dest => dest.OwnershipsQuantity, opt => opt.MapFrom(src => src.Car.OwnershipHistory.Count))
    .ForMember(dest => dest.HasAccident, dest => dest.MapFrom(src => src.Car.HasAccident));

            


            CreateMap<Listing, FeedListingDto>()
                .ForMember(dest => dest.Author, opt => opt.MapFrom(src => src.User))
                .ForMember(dest => dest.Location, opt => opt.MapFrom(src => src.City == null || src.City.Country == null ? null : new LocationDto
                    (
                        new CityDto(src.CityId, src.City.CountryId, src.City.Name),
                        new CountryDto(src.City.Country.Id, src.City.Country.RuName, src.City.Country.EnName, src.City.Country.Code, src.City.Country.FlagImageUrl != null ? src.City.Country.FlagImageUrl.Value : null)
                    )))
                .ForMember(dest => dest.BuyInfoDto, opt => opt.MapFrom(src => src.BuyDetails != null ? src.BuyDetails : null))
                .ForMember(dest => dest.SaleInfoDto, opt => opt.MapFrom(src => src.SaleDetails != null ? src.SaleDetails : null))
                .ForMember(dest => dest.ReactionsQuantity, opt => opt.MapFrom(src => src.Reactions != null ? src.Reactions.Count : 0));

            CreateMap<Listing, AdminListingDto>()
                .ForMember(dest => dest.Author, opt => opt.MapFrom(src => src.User))
                .ForMember(dest => dest.Location, opt => opt.MapFrom(src => src.City == null || src.City.Country == null ? null : new LocationDto
                    (
                        new CityDto(src.CityId, src.City.CountryId, src.City.Name),
                        new CountryDto(src.City.Country.Id, src.City.Country.RuName, src.City.Country.EnName, src.City.Country.Code, src.City.Country.FlagImageUrl != null ? src.City.Country.FlagImageUrl.Value : null)
                    )))
                .ForMember(dest => dest.BuyInfoDto, opt => opt.MapFrom(src => src.BuyDetails != null ? src.BuyDetails : null))
                .ForMember(dest => dest.SaleInfoDto, opt => opt.MapFrom(src => src.SaleDetails != null ? src.SaleDetails : null))
                .ForMember(dest => dest.ReactionsQuantity, opt => opt.MapFrom(src => src.Reactions != null ? src.Reactions.Count : 0))
                .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => src.IsDeleted))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive));

            CreateMap<Listing, BuyListingDto>()
                .ForMember(dest => dest.Author, opt => opt.MapFrom(src => src.User))
                .ForMember(dest => dest.ModelId, opt => opt.MapFrom(src => src.BuyDetails == null ? 0 : src.BuyDetails.ModelId))
                .ForMember(dest => dest.ModelName, opt => opt.MapFrom(src => src.BuyDetails == null ? null : src.BuyDetails.Model != null ? src.BuyDetails.Model.Name : null))
                .ForMember(dest => dest.BrandImageUrl, opt => opt.MapFrom(src => src.BuyDetails == null ? null : src.BuyDetails.Model != null && src.BuyDetails.Model.Brand != null ? src.BuyDetails.Model.Brand.LogoUrl : null))
                .ForMember(dest => dest.Location, opt => opt.MapFrom(src => src.City == null || src.City.Country == null ? null : new LocationDto
                    (
                        new CityDto(src.CityId, src.City.CountryId, src.City.Name),
                        new CountryDto(src.City.Country.Id, src.City.Country.RuName, src.City.Country.EnName, src.City.Country.Code, src.City.Country.FlagImageUrl != null ? src.City.Country.FlagImageUrl.Value : null)
                    )))
                .ForMember(dest => dest.MinPrice, opt => opt.MapFrom(src => src.BuyDetails == null ? 0 : src.BuyDetails.MinPrice))
                .ForMember(dest => dest.MaxPrice, opt => opt.MapFrom(src => src.BuyDetails == null ? 0 : src.BuyDetails.MaxPrice))
                .ForMember(dest => dest.MinReleaseYear, opt => opt.MapFrom(src => src.BuyDetails == null ? 0 : src.BuyDetails.MinReleaseYear))
                .ForMember(dest => dest.MaxReleaseYear, opt => opt.MapFrom(src => src.BuyDetails == null ? 0 : src.BuyDetails.MaxReleaseYear))
                .ForMember(dest => dest.CarColor, opt => opt.MapFrom(src => src.BuyDetails == null ? null : src.BuyDetails.CarColor))
                .ForMember(dest => dest.ReactionsQuantity, opt => opt.MapFrom(src => src.Reactions != null ? src.Reactions.Count : 0));

            CreateMap<Listing, SaleListingDto>()
                .ForMember(dest => dest.Location, opt => opt.MapFrom(src => src.City == null || src.City.Country == null ? null : new LocationDto
                    (
                        new CityDto(src.CityId, src.City.CountryId, src.City.Name),
                        new CountryDto(src.City.Country.Id, src.City.Country.RuName, src.City.Country.EnName, src.City.Country.Code, src.City.Country.FlagImageUrl != null ? src.City.Country.FlagImageUrl.Value : null)
                    )))
                .ForMember(dest => dest.Author, opt => opt.MapFrom(src => src.User))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.SaleDetails == null ? 0 : src.SaleDetails.Price))
                .ForMember(dest => dest.Car, opt => opt.MapFrom(src => src.SaleDetails == null ? null : src.SaleDetails.Car))
                .ForMember(dest => dest.ReactionsQuantity, opt => opt.MapFrom(src => src.Reactions != null ? src.Reactions.Count : 0));

            CreateMap<Car, PublicCarDto>()
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.ImageUrl == null ? null : src.ImageUrl.Value))
                .ForMember(dest => dest.StateNumber, opt => opt.MapFrom(src => src.StateNumber == null ? null : src.StateNumber.Value))
                .ForMember(dest => dest.Model, opt => opt.MapFrom(src => src.Model == null ? null : new ModelShortDto
                (
                    src.Model.Id,
                    src.Model.Name,
                    src.Model.Brand == null ? null : new BrandShortDto(src.Model.Brand.Id, src.Model.Brand.Name, src.Model.Brand.LogoUrl != null ? src.Model.Brand.LogoUrl.Value : null)
                )))
                .ForMember(dest => dest.OwnershipsQuantity, opt => opt.MapFrom(src => src.OwnershipHistory != null ? src.OwnershipHistory.Count : 0));

            CreateMap<CreateBuyListingDto, BuyDetails>();

            CreateMap<CreateBuyListingDto, Listing>()
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => ListingType.Buy))
                .ForMember(dest => dest.UserId, opt => opt.Ignore())
                .ConstructUsing((src, ctx) =>
                {
                    var userId = (int)ctx.Items["UserId"];
                    return new Listing(src.Name, userId, src.Description, ListingType.Buy, src.CityId);
                });

            CreateMap<CreateSaleListingDto, SaleDetails>();

            CreateMap<CreateSaleListingDto, Listing>()
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => ListingType.Sale))
                .ForMember(dest => dest.UserId, opt => opt.Ignore())
                .ConstructUsing((src, ctx) =>
                {
                    var userId = (int)ctx.Items["UserId"];
                    return new Listing(src.Name, userId, src.Description, ListingType.Sale, src.CityId);
                });

            CreateMap<Listing, CreatedListingDto>();
        }
    }
}