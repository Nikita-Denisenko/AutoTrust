using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AutoTrust.Application.Models.DTOs.Requests.Actions.Notification
{
    public record MarkAsReadNotificationsDto
    (
        [Required]
        List<int> NotificationIds
    );
}
