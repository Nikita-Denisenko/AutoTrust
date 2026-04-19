using System.ComponentModel.DataAnnotations;

namespace AutoTrust.Application.Models.DTOs.Requests.UpdateDtos.Review
{
    public record UpdateReviewDto
    (
        [MinLength(1)]
        [MaxLength(80)]
        string? Title,

        [Range(1, 5)]
        int? Stars,

        [MinLength(2)]
        [MaxLength(2000)]
        string? Message
    );
}
