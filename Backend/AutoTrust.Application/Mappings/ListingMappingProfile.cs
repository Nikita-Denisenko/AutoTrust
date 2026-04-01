using AutoMapper;
using AutoTrust.Application.Models.DTOs.Requests.CreateDtos;
using AutoTrust.Application.Models.DTOs.Responses.CreatedDtos;
using AutoTrust.Application.Models.DTOs.Responses.ReadDtos.ListingDtos;
using AutoTrust.Application.Models.DTOs.Responses.ReadDtos.ListingDtos.BuyListingDtos;
using AutoTrust.Application.Models.DTOs.Responses.ReadDtos.ListingDtos.SaleListingDtos;
using AutoTrust.Application.Models.DTOs.Responses.ReadDtos.LocationDTOs;
using AutoTrust.Application.Models.DTOs.Responses.ReadDtos.LocationDTOs.CityDtos;
using AutoTrust.Application.Models.DTOs.Responses.ReadDtos.LocationDTOs.CountryDtos;
using AutoTrust.Domain.Entities;
using AutoTrust.Domain.Enums;

namespace AutoTrust.Application.Mappings
{
    public class ListingMappingProfile : Profile
    {
        public ListingMappingProfile()
        {
            CreateMap<BuyDetails, BuyInfoDto>()
                .ForMember(dest => dest.ModelName, opt => opt.MapFrom(src => src.Model.Name))
                .ForMember(dest => dest.ModelImageUrl, opt => opt.MapFrom(src => src.Model.ImageUrl.Value))
                .ForMember(dest => dest.CarColor, opt => opt.MapFrom(src => src.CarColor));

            CreateMap<SaleDetails, SaleInfoDto>()
                .ForMember(dest => dest.ModelName, opt => opt.MapFrom(src => src.Car.Model.Name))
                .ForMember(dest => dest.CarImageUrl, opt => opt.MapFrom(src => src.Car.ImageUrl.Value))
                .ForMember(dest => dest.CarColor, opt => opt.MapFrom(src => src.Car.Color));

            CreateMap<Listing, FeedListingDto>()
                .ForMember(dest => dest.Author, opt => opt.MapFrom(src => src.User))
                .ForMember(dest => dest.Location, opt => opt
                    .MapFrom(src => new LocationDto
                        (
                            new CityDto
                            (
                                src.CityId,
                                src.City.Name
                            ),
                            new CountryShortDto
                            (
                                src.City.CountryId,
                                src.City.Country.RuName,
                                src.City.Country.EnName,
                                src.City.Country.FlagImageUrl.Value
                            )
                        )
                     )
                )
                .ForMember(dest => dest.BuyInfoDto, opt => opt.MapFrom(src => src.BuyDetails))
                .ForMember(dest => dest.SaleInfoDto, opt => opt.MapFrom(src => src.SaleDetails))
                .ForMember(dest => dest.ReactionsQuantity, opt => opt.MapFrom(src => src.Reactions.Count));

            CreateMap<Listing, AdminListingDto>()
                .IncludeBase<Listing, FeedListingDto>()
                .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => src.IsDeleted))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive));

            CreateMap<Listing, BuyListingDto>()
                .ForMember(dest => dest.Author, opt => opt.MapFrom(src => src.User))
                .ForMember(dest => dest.ModelId, opt => opt.MapFrom(src => src.BuyDetails!.ModelId))
                .ForMember(dest => dest.ModelName, opt => opt.MapFrom(src => src.BuyDetails!.Model.Name))
                .ForMember(dest => dest.ModelImageUrl, opt => opt.MapFrom(src => src.BuyDetails!.Model.ImageUrl.Value))
                .ForMember(dest => dest.Location, opt => opt
                    .MapFrom(src => new LocationDto
                        (
                            new CityDto
                            (
                                src.CityId,
                                src.City.Name
                            ),
                            new CountryShortDto
                            (
                                src.City.CountryId,
                                src.City.Country.RuName,
                                src.City.Country.EnName,
                                src.City.Country.FlagImageUrl.Value
                            )
                        )
                     )
                )
                .ForMember(dest => dest.MinPrice, opt => opt.MapFrom(src => src.BuyDetails!.MinPrice))
                .ForMember(dest => dest.MaxPrice, opt => opt.MapFrom(src => src.BuyDetails!.MaxPrice))
                .ForMember(dest => dest.MinReleaseYear, opt => opt.MapFrom(src => src.BuyDetails!.MinReleaseYear))
                .ForMember(dest => dest.MaxReleaseYear, opt => opt.MapFrom(src => src.BuyDetails!.MaxReleaseYear))
                .ForMember(dest => dest.CarColor, opt => opt.MapFrom(src => src.BuyDetails!.CarColor))
                .ForMember(dest => dest.ReactionsQuantity, opt => opt.MapFrom(src => src.Reactions.Count));

            CreateMap<Listing, SaleListingDto>()
                .ForMember(dest => dest.Location, opt => opt
                    .MapFrom(src => new LocationDto
                        (
                            new CityDto
                            (
                                src.CityId,
                                src.City.Name
                            ),
                            new CountryShortDto
                            (
                                src.City.CountryId,
                                src.City.Country.RuName,
                                src.City.Country.EnName,
                                src.City.Country.FlagImageUrl.Value
                            )
                        )
                     )
                )
                .ForMember(dest => dest.Author, opt => opt.MapFrom(src => src.User))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.SaleDetails!.Price))
                .ForMember(dest => dest.Car, opt => opt.MapFrom(src => src.SaleDetails!.Car))
                .ForMember(dest => dest.ReactionsQuantity, opt => opt.MapFrom(src => src.Reactions.Count));

            CreateMap<CreateBuyListingDto, BuyDetails>();

            CreateMap<CreateBuyListingDto, Listing>()
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => ListingType.Buy))
                .ForMember(dest => dest.UserId, opt => opt.Ignore())
                .ConstructUsing((src, ctx) =>
                {
                    if (!ctx.Items.TryGetValue("UserId", out var userIdObj) || userIdObj is not int userId)
                        throw new InvalidOperationException("UserId not provided");

                    return new Listing(
                        src.Name,
                        userId,
                        src.Description,
                        ListingType.Buy,
                        src.CityId
                    );
                });

            CreateMap<CreateSaleListingDto, SaleDetails>();

            CreateMap<CreateSaleListingDto, Listing>()
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => ListingType.Sale))
                .ForMember(dest => dest.UserId, opt => opt.Ignore())
                .ConstructUsing((src, ctx) =>
                {
                    if (!ctx.Items.TryGetValue("UserId", out var userIdObj) || userIdObj is not int userId)
                        throw new InvalidOperationException("UserId not provided");

                    return new Listing(
                        src.Name,
                        userId,
                        src.Description,
                        ListingType.Sale,
                        src.CityId
                    );
                });

            CreateMap<Listing, CreatedListingDto>();
        }
    }
}
