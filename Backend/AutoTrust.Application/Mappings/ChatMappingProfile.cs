using AutoMapper;
using AutoTrust.Application.Models.DTOs.Responses.CreatedDtos;
using AutoTrust.Application.Models.DTOs.Responses.ReadDtos.ChatDtos;
using AutoTrust.Application.Models.DTOs.Responses.ReadDtos.UserDtos;
using AutoTrust.Domain.Entities;

namespace AutoTrust.Application.Mappings
{
    public class ChatMappingProfile : Profile
    {
        public ChatMappingProfile() 
        {
            CreateMap<ChatParticipant, UserShortDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.User.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.User.Name))
                .ForMember(dest => dest.AvatarUrl, opt => opt.MapFrom(src => src.User.AvatarUrl != null ? src.User.AvatarUrl.Value : null));

            CreateMap<Chat, AdminChatDto>()
                .ForMember(dest => dest.LastMessageId, opt => opt.MapFrom(src => src.Messages.OrderBy(m => m.SentAt).Last().Id))
                .ForMember(dest => dest.Participants, opt => opt.MapFrom(src => src.ChatParticipants));
        }
    }
}
