using AutoMapper;
using AutoTrust.Application.Models.DTOs.Requests.CreateDtos;
using AutoTrust.Application.Models.DTOs.Responses.CreatedDtos;
using AutoTrust.Application.Models.DTOs.Responses.ReadDtos.FollowDtos;
using AutoTrust.Domain.Entities;

namespace AutoTrust.Application.Mappings
{
    public class FollowMappingProfile : Profile
    {
        public FollowMappingProfile() 
        {
            CreateMap<Follow, UserFollowDto>();
            CreateMap<Follow, UserFollowerDto>();
            CreateMap<CreateFollowDto, Follow>();
            CreateMap<Follow, CreatedFollowDto>();
        }
    }
}
