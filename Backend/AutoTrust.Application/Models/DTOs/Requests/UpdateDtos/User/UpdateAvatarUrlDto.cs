using System.ComponentModel.DataAnnotations;

namespace AutoTrust.Application.Models.DTOs.Requests.UpdateDtos.User
{
    public record UpdateAvatarUrlDto
    (
        [Url]
        string? AvatarUrl
    );
}
