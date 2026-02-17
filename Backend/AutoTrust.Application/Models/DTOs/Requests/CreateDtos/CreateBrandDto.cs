using System.ComponentModel.DataAnnotations;

namespace AutoTrust.Application.Models.DTOs.Requests.CreateDtos
{
    public record CreateBrandDto
    (
        [Required]
        [Range(1, int.MaxValue)]
        int CountryId,

        [Required]
        [MaxLength(50)]
        string Name,

        [Required]
        [MaxLength(350)]
        string Description,

        [Required]
        [Url]
        string LogoUrl
    );
}
