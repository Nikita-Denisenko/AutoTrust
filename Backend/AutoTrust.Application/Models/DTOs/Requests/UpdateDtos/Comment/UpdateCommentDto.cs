using System.ComponentModel.DataAnnotations;

namespace AutoTrust.Application.Models.DTOs.Requests.UpdateDtos.Comment
{
    public record UpdateCommentDto
    (
        [Required]
        [MinLength(1)]
        [MaxLength(4000)]
        string Text
    );
}
