namespace AutoTrust.Domain.Entities
{
    public class Account
    {
        public int Id { get; private set; }
        public string Email { get; private set; }
        public string Phone { get; private set; }
        public string PasswordHash { get; private set; }
        public int UserId { get; private set; }
        public User? User { get; private set; }
        public DateTime CreatedAt { get; private set; }

        private Account() { }

        public Account(string email, string phone, string passwordHash)
        {
            Email = email;
            Phone = phone;
            PasswordHash = passwordHash;
            CreatedAt = DateTime.UtcNow;
        }

        public void UpdatePassword(string newPasswordHash)
        {
            PasswordHash = newPasswordHash;
        }
    }
}