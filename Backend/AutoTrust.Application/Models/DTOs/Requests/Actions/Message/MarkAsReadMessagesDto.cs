using System.ComponentModel.DataAnnotations;

namespace AutoTrust.Application.Models.DTOs.Requests.Actions.Message
{
    public record MarkAsReadMessagesDto
    (
        [Required]
        List<int> MessageIds
    );
}
