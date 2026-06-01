using AutoTrust.Application.Models.DTOs.Responses.ReadDtos.MessageDtos;
using AutoTrust.Application.Models.DTOs.Responses.ReadDtos.UserDtos;

namespace AutoTrust.Application.Models.DTOs.Responses.ReadDtos.ChatDtos
{
    public record UserChatListItemDto
    {
        public int Id { get; init; }
        public UserShortDto Companion { get; init; }
        public MessageDto LastMessage { get; init; }
    };
}
