namespace AutoTrust.Domain.Entities
{
    public class Brand
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public string LogoUrl { get; private set; }
        public int CountryId { get; private set; }
        public Country Country { get; private set; }
        public bool IsActive { get; private set; } = true;

        private Brand() { }

        public Brand
        (
            string name,
            string description,
            string logoUrl,
            int countryId
        )
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name cannot be empty!");

            if (string.IsNullOrWhiteSpace(description))
                throw new ArgumentException("Description cannot be empty!");

            if (string.IsNullOrWhiteSpace(logoUrl))
                throw new ArgumentException("Logo URL cannot be empty!");

            if (countryId <= 0)
                throw new ArgumentException("CountryId must be positive!");

            Name = name;
            Description = description;
            LogoUrl = logoUrl;
            CountryId = countryId;
        }

        public void UpdateName(string newName)
        {
            if (string.IsNullOrWhiteSpace(newName))
                throw new ArgumentException("Name cannot be empty!");

            Name = newName;
        }

        public void UpdateDescription(string newDescription)
        {
            if (string.IsNullOrWhiteSpace(newDescription))
                throw new ArgumentException("Description cannot be empty!");

            Description = newDescription;
        }


        public void UpdateLogo(string newLogo)
        {
            if (string.IsNullOrWhiteSpace(newLogo))
                throw new ArgumentException("Logo URL cannot be empty!");

            LogoUrl = newLogo;
        }

        public void Deactivate() => IsActive = false;
        public void Activate() => IsActive = true;
    }
}