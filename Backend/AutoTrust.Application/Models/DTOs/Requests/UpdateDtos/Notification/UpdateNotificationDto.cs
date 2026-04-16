using System.ComponentModel.DataAnnotations;

namespace AutoTrust.Application.Models.DTOs.Requests.UpdateDtos.Notification
{
    public record UpdateNotificationDto
    (
         [MinLength(1)]
         [MaxLength(30)]
         string? Title,

         [MinLength(1)]
         [MaxLength(300)]
         string? Text
    );
}
