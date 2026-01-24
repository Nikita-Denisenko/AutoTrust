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

        private Brand() { }

        public Brand
        (
            string name,
            string description,
            string logoUrl,
            int countryId
        ) 
        {
            Name = name; 
            Description = description; 
            LogoUrl = logoUrl; 
            CountryId = countryId;
        }
    }
}
