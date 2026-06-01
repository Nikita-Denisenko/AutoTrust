namespace AutoTrust.Application.Models.DTOs.Responses.ReadDtos.AccountDtos
{
    public record AccountDto
    {
        public string Email { get; init; }
        public string Phone { get; init; }
        public DateTime CreatedAt { get; init; }
    };
}