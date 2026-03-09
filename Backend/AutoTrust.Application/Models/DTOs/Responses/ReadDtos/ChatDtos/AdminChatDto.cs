using AutoTrust.Application.Models.DTOs.Responses.ReadDtos.UserDtos;

namespace AutoTrust.Application.Models.DTOs.Responses.ReadDtos.ChatDtos
{
    public record AdminChatDto
    (
        int Id,
        int LastMessageId,
        DateTime CreatedAt,
        int PinnedMessageId,
        List<UserShortDto> Participants
    );
}
