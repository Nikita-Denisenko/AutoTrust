using System.Text.RegularExpressions;

namespace AutoTrust.Domain.ValueObjects
{
    public record Phone
    {
        public string Value { get;}

        public Phone(string value) 
        {
            if (!IsValid(value))
                throw new ArgumentException("Phone is not valid.");

            Value = value;
        }

        public static bool IsValid(string phone)
        {
            var regex = new Regex("^\\+?[1-9][0-9]{7,14}$");

            return regex.IsMatch(phone); 
        }

    };
}
