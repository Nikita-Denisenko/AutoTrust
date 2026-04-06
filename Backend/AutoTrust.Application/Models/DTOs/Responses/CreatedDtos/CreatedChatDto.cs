using AutoTrust.Application.Models.DTOs.Responses.ReadDtos.UserDtos;

namespace AutoTrust.Application.Models.DTOs.Responses.CreatedDtos
{
    public record CreatedChatDto
    (
        int Id,
        UserShortDto Companion
    );
}
