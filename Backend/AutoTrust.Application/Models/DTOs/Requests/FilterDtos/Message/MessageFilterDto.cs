namespace AutoTrust.Application.Models.DTOs.Requests.FilterDtos.Message
{
    public record MessageFilterDto
    (
        int Page = 1,
        int Size = 20,
        string SearchText = ""
    );
}
