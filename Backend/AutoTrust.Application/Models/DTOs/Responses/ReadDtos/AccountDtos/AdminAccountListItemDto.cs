namespace AutoTrust.Application.Models.DTOs.Responses.ReadDtos.AccountDtos
{
    public record AdminAccountListItemDto
    {
        public int Id { get; init; }
        public int UserId { get; init; }
        public string Email { get; init; }
    };
}
