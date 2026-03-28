using System.ComponentModel.DataAnnotations;

namespace AutoTrust.Application.Models.DTOs.Requests.Actions.Account
{
    public record ChangePasswordDto
    (
        [Required]
        [MinLength(8)]
        string Password
    );
}
