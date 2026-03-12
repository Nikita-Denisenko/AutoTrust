using static System.Net.WebRequestMethods;

namespace AutoTrust.Domain.Entities
{
    public class Review
    {
        public int Id { get; private set; }
        public int Stars { get; private set; }
        public string Title { get; private set; }
        public string Message { get; private set; }
        public int ReviewerId { get; private set; }
        public User Reviewer { get; private set; }
        public int UserId { get; private set; }
        public User User { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public bool IsDeleted { get; private set; } = false;

        private Review() { }

        public Review
        (
            string title,
            int stars,
            string message,
            int reviewerId,
            int userId
        ) 
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Title cannot be empty!");

            if (stars <= 0)
                throw new ArgumentOutOfRangeException(nameof(stars), stars, "Stars must be positive!");

            if (string.IsNullOrWhiteSpace(message))
                throw new ArgumentException("Message cannot be empty!");

            if (reviewerId <= 0)
                throw new ArgumentOutOfRangeException(nameof(reviewerId), reviewerId, "ReviewerId must be positive");

            if (userId <= 0)
                throw new ArgumentOutOfRangeException(nameof(userId), userId, "UserId must be positive");

            Title = title;
            Stars = stars;
            Message = message;
            ReviewerId = reviewerId;
            UserId = userId;
            CreatedAt = DateTime.UtcNow;
        }

        public void Update(string? newTitle, string? newMessage)
        {
            if (newTitle != null && string.IsNullOrWhiteSpace(newTitle))
                throw new ArgumentException("Title cannot be empty!");

            if (newMessage != null && string.IsNullOrWhiteSpace(newMessage))
                throw new ArgumentException("Message cannot be empty!");

            Title = newTitle ?? Title;
            Message = newMessage ?? Message;
        }

        public void Delete() => IsDeleted = true;
    }
}
