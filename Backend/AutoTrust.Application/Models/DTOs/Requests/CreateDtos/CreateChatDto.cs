using System.ComponentModel.DataAnnotations;

namespace AutoTrust.Application.Models.DTOs.Requests.CreateDtos
{
    public record CreateChatDto
    (
        [Required]
        [Range(1, int.MaxValue)]
        int CompanionId
    );
}