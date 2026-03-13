using System.ComponentModel.DataAnnotations;

namespace AutoTrust.Application.Models.DTOs.Requests.UpdateDtos.Review
{
    public record UpdateReviewDto
    (
        [Range(1, 5)]
        int? Stars,

        [MinLength(2)]
        [MaxLength(1000)]
        string? Message
    );
}
