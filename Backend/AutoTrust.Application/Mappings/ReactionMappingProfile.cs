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
            CreateMap<CreateReactionDto, Reaction>();
            CreateMap<Reaction, CreatedReactionDto>();
        }
    }
}
