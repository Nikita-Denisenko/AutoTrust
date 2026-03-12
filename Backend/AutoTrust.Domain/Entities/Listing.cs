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
        public DateTime? UpdatedAt { get; private set; } = null;
        public int CountryId { get; private set; }
        public Country Country { get; private set; }
        public int CityId { get; private set; }
        public City City { get; private set; }
        public SaleDetails? SaleDetails { get; private set; }
        public BuyDetails? BuyDetails { get; private set; }
        public bool IsDeleted { get; private set; } = false;
        public bool IsActive { get; private set; } = true;

        private Listing() { }

        public Listing
        (
            string name,
            int userId,
            string description,
            ListingType type,
            int countryId,
            int cityId
        ) 
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name cannot be empty!");

            if (userId <= 0)
                throw new ArgumentOutOfRangeException(nameof(userId), userId, "UserId must be positive");

            if (string.IsNullOrWhiteSpace(description))
                throw new ArgumentException("Description cannot be empty!");

            if (countryId <= 0)
                throw new ArgumentOutOfRangeException(
                    nameof(countryId),
                    countryId,
                    "CountryId must be positive!"
                    );

            if (cityId <= 0) 
                throw new ArgumentOutOfRangeException(nameof(cityId), cityId, "CityId must be positive!");

            Name = name;
            UserId = userId;
            Description = description;
            Type = type;
            CreatedAt = DateTime.UtcNow;
            CountryId = countryId;
            CityId = cityId;
        }

        public void UpdateInfo(string? newName, string? newDescription)
        {
            if (newName != null && string.IsNullOrWhiteSpace(newName))
                throw new ArgumentException("Listing Name cannot be empty!");

            if (newDescription != null && string.IsNullOrWhiteSpace(newDescription))
                throw new ArgumentException("Description cannot be empty!");

            Name = newName ?? Name;
            Description = newDescription ?? Description;
            UpdatedAt = DateTime.UtcNow;
        }

        public void Delete() => IsDeleted = true;
        public void Deactivate() => IsActive = false;
        public void Activate() => IsActive = true;
    }
}
