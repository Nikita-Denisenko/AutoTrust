namespace AutoTrust.Application.Models.DTOs.Requests.Actions.Message
{
    public record MarkAsReadMessagesDto
    (
        List<int> MessageIds
    );
}
