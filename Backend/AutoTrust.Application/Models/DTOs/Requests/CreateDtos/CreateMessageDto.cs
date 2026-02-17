using System.ComponentModel.DataAnnotations;

namespace AutoTrust.Application.Models.DTOs.Requests.CreateDtos
{
    public record CreateMessageDto
    (
        [Required]
        [MinLength(1)]
        [MaxLength(4500)]
        string Text,

        [Required]
        [Range(1, int.MaxValue)]
        int ChatId,

        [Required]
        [Range(1, int.MaxValue)]
        int ChatParticipantId
    );
}
