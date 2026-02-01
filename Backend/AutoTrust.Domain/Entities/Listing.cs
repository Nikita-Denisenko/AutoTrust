using AutoTrust.Domain.Enums;

namespace AutoTrust.Domain.Entities
{
    public class Listing
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public int UserId { get; private set; }
        public User User { get; private set; }
        public string Description { get; private set; }
        public ListingType Type { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public int CountryLocationId { get; private set; }
        public Country Country { get; private set; }
        public int CityLocationId { get; private set; }
        public City City { get; private set; }
        public SaleDetails? SaleDetails { get; private set; }
        public BuyDetails? BuyDetails { get; private set; }

        private Listing() { }

        public Listing
        (
            string name,
            int userId,
            string description,
            ListingType type,
            int countryLocationId,
            int cityLocationId
        ) 
        { 
            Name = name;
            UserId = userId;
            Description = description;
            Type = type;
            CreatedAt = DateTime.UtcNow;
            CountryLocationId = countryLocationId;
            CityLocationId = cityLocationId;
        }
    }
}
