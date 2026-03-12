namespace AutoTrust.Domain.Entities
{
    public class City
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public int CountryId { get; private set; }
        public Country Country { get; private set; }

        private City() { }

        public City(string name, int countryId) 
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name cannot be empty!");

            if (countryId <= 0)
                throw new ArgumentOutOfRangeException(
                    nameof(countryId),
                    countryId,
                    "CountryId must be positive!"
                    );

            Name = name;
            CountryId = countryId;
        }
    }
}
