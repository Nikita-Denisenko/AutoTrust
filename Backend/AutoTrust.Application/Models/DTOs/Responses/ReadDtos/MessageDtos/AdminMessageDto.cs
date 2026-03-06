namespace AutoTrust.Application.Models.DTOs.Responses.ReadDtos.MessageDtos
{
    public record AdminMessageDto
    (
        int Id,
        string Text,
        int ChatParticipantId,
        int UserId,
        int ChatId,
        bool IsRead,
        DateTime SentAt,
        string UserAvatarUrl,
        string UserName,
        bool IsDeleted
    );
}
