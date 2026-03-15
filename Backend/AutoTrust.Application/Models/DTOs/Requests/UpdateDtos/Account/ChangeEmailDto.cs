using System.ComponentModel.DataAnnotations;

namespace AutoTrust.Application.Models.DTOs.Requests.UpdateDtos.Account
{
    public record ChangeEmailDto
    (
        [Required]
        [EmailAddress]
        string Email
    );
}
