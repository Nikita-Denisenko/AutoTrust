using AutoTrust.Application.Models.DTOs.Responses.ReadDtos.ChatParticipantDtos;
using AutoTrust.Application.Models.DTOs.Responses.ReadDtos.MessageDtos;

namespace AutoTrust.Application.Models.DTOs.Responses.ReadDtos.ChatDtos
{
    public record UserChatInfoDto
    (
        ChatParticipantDto Companion,
        MessageDto PinnedMessage
    );
}