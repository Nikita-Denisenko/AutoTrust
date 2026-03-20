using AutoTrust.Domain.ValueObjects;

namespace AutoTrust.Domain.Entities
{
    public class Brand
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public Url LogoUrl { get; private set; }
        public int CountryId { get; private set; }
        public Country Country { get; private set; }
        public bool IsDeleted { get; private set; } = false;
        public ICollection<Model> Models { get; private set; } = [];    

        private Brand() { }

        public Brand
        (
            string name,
            string description,
            Url logoUrl,
            int countryId
        )
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name cannot be empty!");

            if (string.IsNullOrWhiteSpace(description))
                throw new ArgumentException("Description cannot be empty!");

            if (countryId <= 0)
                throw new ArgumentOutOfRangeException(
                    nameof(countryId), 
                    countryId, 
                    "CountryId must be positive!"
                    );

            Name = name;
            Description = description;
            LogoUrl = logoUrl;
            CountryId = countryId;
        }

        public void Update(string? newName, string? newDescription, Url? newLogoUrl)
        {
            if (newName != null && string.IsNullOrWhiteSpace(newName))
                throw new ArgumentException("Name cannot be empty!");

            if (newDescription != null && string.IsNullOrWhiteSpace(newDescription))
                throw new ArgumentException("Description cannot be empty!");

            Name = newName ?? Name;
            Description = newDescription ?? Description;
            LogoUrl = newLogoUrl ?? LogoUrl;

        }
        public void Delete() => IsDeleted = true; 
    }
}