using AutoTrust.Application.Models.DTOs.Responses.ReadDtos.UserDtos;

namespace AutoTrust.Application.Models.DTOs.Responses.ReadDtos.ChatParticipantDtos
{
    public record ChatParticipantDto
    (
        int Id,
        int ChatId,
        UserShortDto User
    );
}