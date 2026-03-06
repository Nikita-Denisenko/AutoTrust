using System.ComponentModel.DataAnnotations;

namespace AutoTrust.Application.Models.DTOs.Requests.CreateDtos
{
    public record CreateCommentDto
    (
        [Required]
        [Range(1, int.MaxValue)]
        int UserId,

        [Required]
        [Range(1, int.MaxValue)]
        int ListingId,

        [Required]
        [MinLength(1)]
        [MaxLength(4000)]
        string Text
    );
}
