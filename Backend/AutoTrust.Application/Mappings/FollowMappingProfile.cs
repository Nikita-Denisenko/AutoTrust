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

            CreateMap<CreateFollowDto, Follow>()
                .ForMember(dest => dest.FollowerId, opt => opt.Ignore())
                .ConstructUsing((src, ctx) =>
                {
                    if (!ctx.Items.TryGetValue("FollowerId", out var followerIdObj) || followerIdObj is not int followerId)
                        throw new InvalidOperationException("FollowerId not provided");

                    return new Follow(
                        followerId,
                        src.TargetId
                    );
                });

            CreateMap<Follow, CreatedFollowDto>();
        }
    }
}
