using System.ComponentModel.DataAnnotations;

namespace AutoTrust.Application.Models.DTOs.Requests.Actions.Account
{
    public record ChangeEmailDto
    (
        [Required]
        [EmailAddress]
        string Email
    );
}
