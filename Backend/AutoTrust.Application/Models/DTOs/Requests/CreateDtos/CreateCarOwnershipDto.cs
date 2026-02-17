using System.ComponentModel.DataAnnotations;

namespace AutoTrust.Application.Models.DTOs.Requests.CreateDtos
{
    public record CreateCarOwnershipDto
    (
        [Required]
        [Range(1, int.MaxValue)]
        int UserId,

        [Required]
        [Range(0, int.MaxValue)]
        decimal MileageBefore,

        [Required]
        [Range(0, int.MaxValue)]
        decimal MileageAfter,

        [Required]
        DateOnly FromDate,

        [Required]
        DateOnly ToDate,

        [Required]
        [Range(1, int.MaxValue)]
        int CarId
    );
}