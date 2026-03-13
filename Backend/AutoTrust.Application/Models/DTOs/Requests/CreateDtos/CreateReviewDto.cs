using System.ComponentModel.DataAnnotations;

namespace AutoTrust.Application.Models.DTOs.Requests.CreateDtos
{
    public record CreateReviewDto
    (
        [Required]
        [MinLength(1)]
        [MaxLength(80)]
        string Title,

        [Required]
        [Range(1, 5)]
        int Stars,

        [Required]
        [MinLength(2)]
        [MaxLength(2000)]
        string Message,

        [Required]
        [Range(1, int.MaxValue)]
        int ReviewerId,

        [Required]
        [Range(1, int.MaxValue)]
        int UserId
    );
}