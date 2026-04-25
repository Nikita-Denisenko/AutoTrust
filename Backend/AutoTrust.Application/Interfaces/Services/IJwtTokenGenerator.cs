using AutoTrust.Domain.Entities;

namespace AutoTrust.Application.Interfaces.Services
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(Account account);
    }
}