using AutoTrust.Application.Models.DTOs.Responses.ReadDtos.UserDtos;

namespace AutoTrust.Application.Models.DTOs.Responses.ReadDtos.MessageDtos
{
    public record MessageDto
    (
        int Id,
        string Text,
        UserShortDto User,
        bool IsRead,
        DateTime SentAt
    );
}