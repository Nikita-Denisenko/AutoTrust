using System.ComponentModel.DataAnnotations;

namespace AutoTrust.Application.Models.DTOs.Requests.UpdateDtos
{
    public record UpdateChatDto
    (
        [MinLength(1)]
        [MaxLength(50)]
        string? Name
    );
}
