using System.ComponentModel.DataAnnotations;

namespace AutoTrust.Application.Models.DTOs.Requests.UpdateDtos.User
{
    public record UpdateAvatarUrl
    (
        [Url]
        string? AvatarUrl
    );
}
