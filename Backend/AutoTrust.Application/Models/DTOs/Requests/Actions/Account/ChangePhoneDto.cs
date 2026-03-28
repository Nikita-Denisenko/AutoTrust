using System.ComponentModel.DataAnnotations;

namespace AutoTrust.Application.Models.DTOs.Requests.Actions.Account
{
    public record ChangePhoneDto
    (
        [Required]
        [Phone]
        string Phone
    );
}
