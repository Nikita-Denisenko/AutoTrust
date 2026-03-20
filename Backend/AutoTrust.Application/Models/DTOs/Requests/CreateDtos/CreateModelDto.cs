using System.ComponentModel.DataAnnotations;

namespace AutoTrust.Application.Models.DTOs.Requests.CreateDtos
{
    public record CreateModelDto
    (
        [Required]
        [MinLength(2)]
        [MaxLength(50)]
        string Name,

        [Required]
        [MinLength(1)]
        [MaxLength(900)]
        string Description,

        [Required]
        [Url]
        string ImageUrl,

        [Required]
        [Range(1, int.MaxValue)]
        int BrandId
    );
}