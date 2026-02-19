namespace AutoTrust.Application.Models.DTOs.Responses.CreatedDtos
{
    public record CreatedChatParticipantDto
    (
        int Id,
        int UserId,
        int ChatId
    );
}
