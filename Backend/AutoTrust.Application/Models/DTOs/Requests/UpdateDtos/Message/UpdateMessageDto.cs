using System.ComponentModel.DataAnnotations;

namespace AutoTrust.Application.Models.DTOs.Requests.UpdateDtos.Message
{
    public record UpdateMessageDto
    (
        [MinLength(1)]
        [MaxLength(4500)]
        string Text
    );
}