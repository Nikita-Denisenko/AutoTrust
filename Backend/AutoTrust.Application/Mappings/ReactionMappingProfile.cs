using AutoMapper;
using AutoTrust.Application.Models.DTOs.Requests.CreateDtos;
using AutoTrust.Application.Models.DTOs.Responses.CreatedDtos;
using AutoTrust.Application.Models.DTOs.Responses.ReadDtos.ReactionDtos;
using AutoTrust.Domain.Entities;

namespace AutoTrust.Application.Mappings
{
    public class ReactionMappingProfile : Profile
    {
        public ReactionMappingProfile() 
        {
            CreateMap<Reaction, AdminListingReactionDto>();
            CreateMap<Reaction, AdminUserReactionDto>();
            CreateMap<Reaction, ReactionDto>();

            CreateMap<CreateReactionDto, Reaction>()
                .ForMember(dest => dest.UserId, opt => opt.Ignore())
                .ConstructUsing((src, ctx) =>
                {
                    if (!ctx.Items.TryGetValue("UserId", out var userIdObj) || userIdObj is not int userId)
                        throw new InvalidOperationException("UserId not provided");

                    return new Reaction(
                        src.Emoji,
                        src.Name,
                        userId,
                        src.ListingId
                    );
                });

            CreateMap<Reaction, CreatedReactionDto>();
        }
    }
}
