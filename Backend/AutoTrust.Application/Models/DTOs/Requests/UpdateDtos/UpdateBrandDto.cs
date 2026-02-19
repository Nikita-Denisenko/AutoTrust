using System.ComponentModel.DataAnnotations;

namespace AutoTrust.Application.Models.DTOs.Requests.UpdateDtos
{
    public record UpdateBrandDto
    (
        [MaxLength(50)]
        string? Name,

        [MaxLength(350)]
        string? Description,

        [Url]
        string? LogoUrl
    );
}
