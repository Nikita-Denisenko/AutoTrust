using System.ComponentModel.DataAnnotations;

namespace AutoTrust.Application.Models.DTOs.Requests.Actions.Message
{
    public record DeleteMessagesDto
    (
        [Required] 
        List<int> MessageIds
    );
}
