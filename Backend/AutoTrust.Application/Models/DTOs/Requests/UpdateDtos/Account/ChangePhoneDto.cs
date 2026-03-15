using System.ComponentModel.DataAnnotations;

namespace AutoTrust.Application.Models.DTOs.Requests.UpdateDtos.Account
{
    public record ChangePhoneDto
    (
        [Required]
        [Phone]
        string Phone
    );
}
