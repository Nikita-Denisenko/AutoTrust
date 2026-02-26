namespace AutoTrust.Application.Models.DTOs.Responses.ReadDtos.ChatParticipantDtos
{
    public record ChatParticipantDto
    (
        int Id,
        int UserId,
        int ChatId,
        string? UserAvatarUrl,
        string UserName
    );
}