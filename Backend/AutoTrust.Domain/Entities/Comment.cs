namespace AutoTrust.Domain.Entities
{
    public class Comment
    {
        public int Id { get; private set; }
        public int UserId { get; private set; }
        public User User { get; private set; }
        public int ListingId { get; private set; }
        public Listing Listing { get; private set; }
        public string Text { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public bool IsBlocked { get; private set; } = false;

        public bool IsDeleted { get; private set; } = false;

        private Comment() { }

        public Comment
        (
            int userId,
            int listingId,
            string text
        )
        {
            UserId = userId;
            ListingId = listingId;
            Text = text;
            CreatedAt = DateTime.UtcNow;
        }
    }
}
