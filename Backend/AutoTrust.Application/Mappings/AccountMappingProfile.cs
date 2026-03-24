using AutoMapper;
using AutoTrust.Application.Models.DTOs.Responses.ReadDtos.AccountDtos;
using AutoTrust.Domain.Entities;

namespace AutoTrust.Application.Mappings
{
    public class AccountMappingProfile : Profile
    {
        public AccountMappingProfile() 
        {
            CreateMap<Account, AccountDto>()
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email.Value))
                .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.Phone.Value));

            CreateMap<Account, AdminAccountDto>()
                .IncludeBase<Account, AccountDto>();

            CreateMap<Account, AdminAccountListItemDto>()
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email.Value));
        }
    }
}
