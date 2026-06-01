using AutoTrust.Application.Models.DTOs.Responses.ReadDtos.MessageDtos;
using AutoTrust.Application.Models.DTOs.Responses.ReadDtos.UserDtos;

namespace AutoTrust.Application.Models.DTOs.Responses.ReadDtos.ChatDtos
{
    public record UserChatInfoDto
    {
        public UserShortDto Companion { get; init; }
        public MessageDto PinnedMessage { get; init; }
    };
}