using AutoTrust.Domain.Constants;
using AutoTrust.Domain.ValueObjects;

namespace AutoTrust.Domain.Entities
{
    public class Account
    {
        public int Id { get; private set; }
        public Email Email { get; private set; }
        public Phone Phone { get; private set; }
        public string PasswordHash { get; private set; }
        public int UserId { get; private set; }
        public User User { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public bool IsDeleted {  get; private set; } = false;
        public string Role { get; private set; } = Roles.User;
        

        private Account() { }

        public Account(Email email, Phone phone, string passwordHash, int userId)
        {
            Email = email ?? throw new ArgumentNullException(nameof(email));
            Phone = phone ?? throw new ArgumentNullException(nameof(phone));

            if (string.IsNullOrWhiteSpace(passwordHash))
                throw new ArgumentException("Password hash cannot be empty");

            if (userId <= 0)
                throw new ArgumentOutOfRangeException(nameof(userId), userId, "UserId must be positive");

            PasswordHash = passwordHash;
            CreatedAt = DateTime.UtcNow;
            UserId = userId;
        }

        public void ChangePassword(string newPasswordHash)
        {
            if (string.IsNullOrWhiteSpace(newPasswordHash))
                throw new ArgumentException("Password hash cannot be empty");

            PasswordHash = newPasswordHash;
        }

        public void ChangePhone(Phone newPhone)
        {
            Phone = newPhone ?? throw new ArgumentNullException(nameof(newPhone));
        }

        public void Delete() => IsDeleted = true;
    }
}
