using System.ComponentModel.DataAnnotations;

namespace AutoTrust.Application.Models.DTOs.Requests.UpdateDtos.Account
{
    public record UpdateAccountDto
    (
        [EmailAddress]
        string? Email,

        [Phone]
        string? Phone,

        [MinLength(8)]
        string? PasswordHash
    );
}
