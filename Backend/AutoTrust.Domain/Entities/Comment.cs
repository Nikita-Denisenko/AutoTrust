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
        public DateTime? UpdatedAt { get; private set; } = null;
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
            if (userId <= 0)
                throw new ArgumentException("UserId must be positive!");

            if (listingId <= 0)
                throw new ArgumentException("ListingId must be positive!");

            if (string.IsNullOrWhiteSpace(text))
                throw new ArgumentException("Comment cannot be empty!");

            UserId = userId;
            ListingId = listingId;
            Text = text;
            CreatedAt = DateTime.UtcNow;
        }

        public void UpdateText(string newText)
        {
            if (string.IsNullOrWhiteSpace(newText))
                throw new ArgumentException("Comment cannot be empty!");

            Text = newText;
            UpdatedAt = DateTime.UtcNow;
        }

        public void Delete() => IsDeleted = true;
        public void Block() => IsBlocked = true;
        public void UnBlock() => IsBlocked = false;
    }
}
