namespace AutoTrust.Application.Models.DTOs.Responses.ReadDtos.AccountDtos
{
    public record AdminAccountListItemDto
    (
        int Id,
        int UserId,
        string Email
    );
}
