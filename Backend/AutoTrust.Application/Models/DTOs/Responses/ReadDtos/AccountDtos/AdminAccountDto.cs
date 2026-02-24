namespace AutoTrust.Application.Models.DTOs.Responses.ReadDtos.AccountDtos
{
    public record AdminAccountDto
    (
        int Id,
        int UserId,
        string Email,
        string Phone,
        DateTime CreatedAt
    );
}
