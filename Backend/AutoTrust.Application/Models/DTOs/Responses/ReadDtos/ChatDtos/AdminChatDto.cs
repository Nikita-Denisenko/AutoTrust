using AutoTrust.Application.Models.DTOs.Responses.ReadDtos.ChatParticipantDtos;

namespace AutoTrust.Application.Models.DTOs.Responses.ReadDtos.ChatDtos
{
    public record AdminChatDto
    (
        int Id,
        int LastMessageId,
        DateTime CreatedAt,
        int PinnedMessageId,
        List<ChatParticipantDto> ChatParticipants
    );
}
