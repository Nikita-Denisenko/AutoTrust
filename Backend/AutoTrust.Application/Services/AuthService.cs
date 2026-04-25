using AutoTrust.Application.Interfaces.Repositories;
using AutoTrust.Application.Interfaces.Services;
using AutoTrust.Application.Models.DTOs.AuthDtos;
using AutoTrust.Domain.Entities;
using AutoTrust.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace AutoTrust.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IRepository<User> _userRepo;
        private readonly IRepository<Account> _accountRepo;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;

        public AuthService(
            IRepository<User> userRepo,
            IRepository<Account> accountRepo,
            IPasswordHasher passwordHasher,
            IJwtTokenGenerator jwtTokenGenerator)
        {
            _userRepo = userRepo;
            _accountRepo = accountRepo;
            _passwordHasher = passwordHasher;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        public async Task<AuthResponseDto> RegisterAsync(RegisterDto dto, CancellationToken ct)
        {
            var existingAccount = await _accountRepo.GetQuery()
                .FirstOrDefaultAsync(a => a.Email.Value == dto.Email, ct);

            if (existingAccount != null)
                throw new InvalidOperationException("Account already exists");

            var birthDate = BirthDate.Create(dto.BirthDate);
            var user = new User(
                dto.Name,
                dto.Surname,
                dto.Patronymic,
                birthDate,
                dto.Gender,
                dto.CityId
            );

            await _userRepo.AddAsync(user, ct);
            await _userRepo.SaveChangesAsync(ct);

            var hashedPassword = _passwordHasher.HashPassword(dto.Password);
            var email = new Email(dto.Email);
            var phone = new Phone(dto.Phone);
            var account = new Account(email, phone, hashedPassword, user.Id);

            await _accountRepo.AddAsync(account, ct);
            await _accountRepo.SaveChangesAsync(ct);

            var token = _jwtTokenGenerator.GenerateToken(account);
            return new AuthResponseDto(token, account.Email.Value, account.Role);
        }

        public async Task<AuthResponseDto> LoginAsync(LoginDto dto, CancellationToken ct)
        {
            var account = await _accountRepo.GetQuery()
                .Include(a => a.User)
                .FirstOrDefaultAsync(a => a.Email.Value == dto.Email, ct);

            if (account == null || !_passwordHasher.VerifyPassword(dto.Password, account.PasswordHash))
                throw new InvalidOperationException("Invalid email or password");

            var token = _jwtTokenGenerator.GenerateToken(account);
            return new AuthResponseDto(token, account.Email.Value, account.Role);
        }
    }
}