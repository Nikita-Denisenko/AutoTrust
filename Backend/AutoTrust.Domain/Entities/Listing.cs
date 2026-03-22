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
        public int CityId { get; private set; }
        public City City { get; private set; }
        public SaleDetails? SaleDetails { get; private set; }
        public BuyDetails? BuyDetails { get; private set; }
        public bool IsDeleted { get; private set; } = false;
        public bool IsActive { get; private set; } = true;
        public ICollection<Comment> Comments {  get; private set; }
        public ICollection<Reaction> Reactions { get; private set; }

        private Listing() { }

        public Listing
        (
            string name,
            int userId,
            string description,
            ListingType type,
            int cityId
        ) 
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name cannot be empty!");

            if (userId <= 0)
                throw new ArgumentOutOfRangeException(nameof(userId), userId, "UserId must be positive");

            if (string.IsNullOrWhiteSpace(description))
                throw new ArgumentException("Description cannot be empty!");

            if (cityId <= 0) 
                throw new ArgumentOutOfRangeException(nameof(cityId), cityId, "CityId must be positive!");

            Name = name;
            UserId = userId;
            Description = description;
            Type = type;
            CreatedAt = DateTime.UtcNow;
            CityId = cityId;
        }

        public void SetBuyDetails(BuyDetails buyDetails)
        {
            if (Type != ListingType.Buy)
                throw new InvalidOperationException("Cannot add buy details to non-buy listing");
            BuyDetails = buyDetails;
        }

        public void SetSaleDetails(SaleDetails saleDetails)
        {
            if (Type != ListingType.Sale)
                throw new InvalidOperationException("Cannot add sale details to non-sale listing");
            SaleDetails = saleDetails;
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
