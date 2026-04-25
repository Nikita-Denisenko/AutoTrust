using AutoTrust.Application.Models.DTOs.AuthDtos;

namespace AutoTrust.Application.Interfaces.Services
{
    public interface IAuthService
    {
        public Task<AuthResponseDto> RegisterAsync(RegisterDto dto, CancellationToken cancellationToken);
        public Task<AuthResponseDto> LoginAsync(LoginDto dto, CancellationToken cancellationToken);
    }
}