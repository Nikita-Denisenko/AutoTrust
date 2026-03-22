using AutoMapper;
using AutoTrust.Application.Models.DTOs.Requests.CreateDtos;
using AutoTrust.Application.Models.DTOs.Responses.CreatedDtos;
using AutoTrust.Application.Models.DTOs.Responses.ReadDtos.CommentDtos;
using AutoTrust.Domain.Entities;

namespace AutoTrust.Application.Mappings
{
    public class CommentMappingProfile : Profile
    {
        public CommentMappingProfile() 
        {
            CreateMap<Comment, AdminListingCommentDto>();
            CreateMap<Comment, AdminUserCommentDto>();
            CreateMap<Comment, CommentDto>();
            CreateMap<CreateCommentDto, CommentDto>();
            CreateMap<Comment, CreatedCommentDto>();
        }
    }
}
