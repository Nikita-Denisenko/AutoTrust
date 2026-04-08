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
            CreateMap<Comment, AdminCommentDto>();
            CreateMap<Comment, CommentDto>();

            CreateMap<CreateCommentDto, Comment>()
                .ForMember(dest => dest.UserId, opt => opt.Ignore())
                .ConstructUsing((src, ctx) =>
                {
                    if (!ctx.Items.TryGetValue("UserId", out var userIdObj) || userIdObj is not int userId)
                        throw new InvalidOperationException("UserId not provided");

                    return new Comment(
                        userId,
                        src.ListingId,
                        src.Text
                    );
                });

            CreateMap<Comment, CreatedCommentDto>();
        }
    }
}
