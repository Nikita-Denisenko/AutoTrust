namespace AutoTrust.Domain.Entities
{
    public class Reaction
    {
        public int Id { get; private set; }
        public string Emoji { get; private set; }
        public string Name { get; private set; }
        public int UserId { get; private set; }
        public User User { get; private set; }
        public int ListingId { get; private set; }
        public Listing Listing { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public bool IsDeleted { get; private set; }

        private Reaction() { }

        public Reaction
        (
            string emoji, 
            string name, 
            int userId, 
            int listingId
        )
        {
            if (string.IsNullOrWhiteSpace(emoji))
                throw new ArgumentException("Emoji cannot be empty!");

            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name cannot be empty!");

            if (userId <= 0)
                throw new ArgumentOutOfRangeException(nameof(userId), userId, "UserId must be positive");

            if (listingId <= 0)
                throw new ArgumentOutOfRangeException(nameof(listingId), listingId, "ListingId must be positive!");

            Emoji = emoji;
            Name = name;
            UserId = userId;
            ListingId = listingId;
            CreatedAt = DateTime.UtcNow;
        }

        public void Delete() => IsDeleted = true;
    }
}
