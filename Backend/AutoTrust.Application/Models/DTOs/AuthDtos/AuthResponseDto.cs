namespace AutoTrust.Application.Models.DTOs.AuthDtos
{
    public record AuthResponseDto
    (
        string Token,
        string Email,
        string Role
    );
}