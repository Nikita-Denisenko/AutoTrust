using System.ComponentModel.DataAnnotations;

namespace AutoTrust.Application.Models.DTOs.Requests.UpdateDtos.Account
{
    public record ChangePasswordDto
    (
        [Required]
        [MinLength(8)]
        string Password
    );
}
