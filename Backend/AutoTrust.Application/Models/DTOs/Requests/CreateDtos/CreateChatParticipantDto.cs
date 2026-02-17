using System.ComponentModel.DataAnnotations;

namespace AutoTrust.Application.Models.DTOs.Requests.CreateDtos
{
    public record CreateChatParticipantDto
    (
        [Required]
        [Range(1, int.MaxValue)]
        int UserId,

        [Required]
        [Range(1, int.MaxValue)]
        int ChatId
    );
}
