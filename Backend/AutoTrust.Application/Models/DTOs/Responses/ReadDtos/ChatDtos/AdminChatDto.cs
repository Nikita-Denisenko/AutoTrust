using AutoTrust.Application.Models.DTOs.Responses.ReadDtos.MessageDtos;
using AutoTrust.Application.Models.DTOs.Responses.ReadDtos.UserDtos;

namespace AutoTrust.Application.Models.DTOs.Responses.ReadDtos.ChatDtos
{
    public record AdminChatDto
    (
        int Id,
        int LastMessageId,
        DateTime CreatedAt,
        MessageDto PinnedMessage,
        List<UserShortDto> Participants
    );
}
