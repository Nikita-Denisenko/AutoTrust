namespace AutoTrust.Domain.Entities
{
    public class Review
    {
        public int Id { get; private set; }
        public int Stars { get; private set; }
        public string Message { get; private set; }
        public int ReviewerId { get; private set; }
        public User Reviewer { get; private set; }
        public int UserId { get; private set; }
        public User User { get; private set; }
        public DateTime CreatedAt { get; private set; }

        private Review() { }

        public Review
        (
           int stars,
           string message,
           int reviewerId,
           int userId
        ) 
        { 
            Stars = stars;
            Message = message;
            ReviewerId = reviewerId;
            UserId = userId;
            CreatedAt = DateTime.UtcNow;
        }
    }
}
