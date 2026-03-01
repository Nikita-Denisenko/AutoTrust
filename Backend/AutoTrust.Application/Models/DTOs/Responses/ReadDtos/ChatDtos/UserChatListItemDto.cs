using AutoTrust.Application.Models.DTOs.Responses.ReadDtos.MessageDtos;

namespace AutoTrust.Application.Models.DTOs.Responses.ReadDtos.ChatDtos
{
    public record UserChatListItemDto
    (
        int Id,
        string Name,
        string CompanionAvatarUrl,
        MessageDto LastMessage
    );
}
