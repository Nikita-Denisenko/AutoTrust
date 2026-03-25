using System.ComponentModel.DataAnnotations;

namespace AutoTrust.Application.Models.DTOs.AuthDtos
{
    public record LoginDto
    (
        [Required]
        [EmailAddress]
        string Email,

        [Required]
        [MinLength(8)]
        [MaxLength(64)]
        string Password
    );
}
