using System.ComponentModel.DataAnnotations;

namespace AutoTrust.Application.Models.DTOs.Requests.CreateDtos
{
    public record CreateReviewDto
    (
        [Required]
        [Range(1, 5)]
        int Stars,

        [Required]
        [MinLength(2)]
        [MaxLength(1000)]
        string Message,

        [Required]
        [Range(1, int.MaxValue)]
        int ReviewerId,

        [Required]
        [Range(1, int.MaxValue)]
        int UserId
    );
}
