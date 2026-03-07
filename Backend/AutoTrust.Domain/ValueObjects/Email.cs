using System.Net.Mail;

namespace AutoTrust.Domain.ValueObjects
{
    public record Email
    {
        public string Value { get; }

        public Email(string value)
        {
            if (!IsValid(value))
                throw new ArgumentException("Email is not valid.");
            Value = value;
        }

        public static bool IsValid(string email) 
        {
            try
            {
                _ = new MailAddress(email);
                return true;
            }
            catch(FormatException)
            {
                return false;
            }
        }
    }
}