namespace AutoTrust.Application.Models.DTOs.Responses.ReadDtos.AccountDtos
{
    public record AdminAccountDto
    {
        public int Id { get; init; }
        public int UserId { get; init; }
        public string Email { get; init; }
        public string Phone {  get; init; }
        public DateTime CreatedAt { get; init; }
        public bool IsDeleted { get; init; }
    };
}
