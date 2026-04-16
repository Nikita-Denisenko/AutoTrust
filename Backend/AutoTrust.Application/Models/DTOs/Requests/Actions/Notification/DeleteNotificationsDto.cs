using System.ComponentModel.DataAnnotations;

namespace AutoTrust.Application.Models.DTOs.Requests.Actions.Notification
{
    public record DeleteNotificationsDto
    (
        [Required]
        List<int> NotificationIds
    );
}
