using AutoTrust.Application.Models.DTOs.Responses.ReadDtos.UserDtos;

namespace AutoTrust.Application.Models.DTOs.Responses.ReadDtos.MessageDtos
{
    public record AdminMessageDto
    (
        int Id,
        string Text,
        int ChatId,
        bool IsRead,
        DateTime SentAt,
        UserShortDto User,
        bool IsDeleted
    );
}
