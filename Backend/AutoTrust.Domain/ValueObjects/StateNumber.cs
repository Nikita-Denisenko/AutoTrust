using System.Text.RegularExpressions;

namespace AutoTrust.Domain.ValueObjects
{
    public record StateNumber
    {
        public string Value { get; private set; }

        public StateNumber(string stateNumber)
        {
            if (!IsValid(stateNumber))
                throw new ArgumentException("State number is not valid!");

            Value = stateNumber;
        }

        public static bool IsValid(string stateNumber) 
        {
            var regex = new Regex("^[А-Я]{1}\\d{3}[А-Я]{2}\\d{2,3}$");

            return regex.IsMatch(stateNumber);
        }
    }
}
