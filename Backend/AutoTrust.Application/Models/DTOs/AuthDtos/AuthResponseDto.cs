namespace AutoTrust.Application.Models.DTOs.AuthDtos
{
    public record AuthResponseDto
    {
        public string Token { get; init; }
        public string Email { get; init; }
        public  string Role { get; init; }
    };
}