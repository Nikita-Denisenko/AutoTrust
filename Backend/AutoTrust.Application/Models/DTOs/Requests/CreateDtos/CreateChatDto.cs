using System.ComponentModel.DataAnnotations;

namespace AutoTrust.Application.Models.DTOs.Requests.CreateDtos
{
    public record CreateChatDto
    (
        [MinLength(1)]
        [MaxLength(50)]
        string? Name
    );
}
