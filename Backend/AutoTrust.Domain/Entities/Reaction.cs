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

        public DateTime ReactedAt { get; private set; }

        private Reaction() { }

        public Reaction
        (
            string emoji, 
            string name, 
            int userId, 
            int listingId
        )
        {
            Emoji = emoji;
            Name = name;
            UserId = userId;
            ListingId = listingId;
            ReactedAt = DateTime.UtcNow;
        }
    }
}
