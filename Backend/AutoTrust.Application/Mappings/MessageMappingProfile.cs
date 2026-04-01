using AutoMapper;
using AutoTrust.Application.Models.DTOs.Requests.CreateDtos;
using AutoTrust.Application.Models.DTOs.Responses.CreatedDtos;
using AutoTrust.Application.Models.DTOs.Responses.ReadDtos.MessageDtos;
using AutoTrust.Domain.Entities;

namespace AutoTrust.Application.Mappings
{
    public class MessageMappingProfile : Profile
    {
        public MessageMappingProfile() 
        {
            CreateMap<Message, MessageDto>()
                .IncludeMembers(src => src.User);

            CreateMap<Message, AdminMessageDto>()
                .IncludeBase<Message, MessageDto>()
                .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => src.IsDeleted));

            CreateMap<CreateMessageDto, Message>()
                .ForMember(dest => dest.UserId, opt => opt.Ignore())
                .ConstructUsing((src, ctx) =>
                {
                    if (!ctx.Items.TryGetValue("UserId", out var userIdObj) || userIdObj is not int userId)
                        throw new InvalidOperationException("UserId not provided");

                    return new Message(
                        src.Text,
                        src.ChatId,
                        userId
                    );
                });

            CreateMap<Message, CreatedMessageDto>();
        }    
    }
}
