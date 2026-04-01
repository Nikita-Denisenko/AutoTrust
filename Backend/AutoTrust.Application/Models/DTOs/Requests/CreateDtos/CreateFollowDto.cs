using System.ComponentModel.DataAnnotations;

namespace AutoTrust.Application.Models.DTOs.Requests.CreateDtos
{
    public record CreateFollowDto
    (
        [Required]
        [Range(1, int.MaxValue)]
        int TargetId
    );
}
