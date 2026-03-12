namespace AutoTrust.Domain.ValueObjects
{
    public record BirthDate
    {
        public DateOnly Value { get; }

        private BirthDate(DateOnly value)
        {
            Value = value;
        }

        public static BirthDate Create(DateOnly value)
        {
            var today = DateOnly.FromDateTime(DateTime.UtcNow);

            if (value > today)
                throw new ArgumentOutOfRangeException(nameof(value), "Birth date cannot be in the future");

            var age = today.Year - value.Year;
            if (value > today.AddYears(-age))
                age--;

            if (age < 18)
                throw new ArgumentOutOfRangeException(nameof(value), "User must be at least 18 years old");

            return new BirthDate(value);
        }
    }
}