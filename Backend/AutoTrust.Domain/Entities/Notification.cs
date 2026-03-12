using static System.Net.Mime.MediaTypeNames;

namespace AutoTrust.Domain.Entities
{
    public class Notification
    {
        public int Id { get; private set; }
        public string Title { get; private set; }
        public string Text { get; private set; }
        public int UserId { get; private set; }
        public User User { get; private set; }
        public bool IsRead { get; private set; } = false;
        public DateTime CreatedAt { get; private set; }
        public bool IsDeleted { get; private set; } = false;

        private Notification() { }

        public Notification
        (
            string title, 
            string text, 
            int userId
        )
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Title cannot be empty");

            if (string.IsNullOrWhiteSpace(text))
                throw new ArgumentException("Text cannot be empty");

            if (userId <= 0)
                throw new ArgumentOutOfRangeException(nameof(userId), userId, "UserId must be positive");

            Title = title;
            Text = text;
            UserId = userId;
            CreatedAt = DateTime.UtcNow;
        }

        public void Update(string? newTitle, string? newText)
        {
            if (newTitle != null && string.IsNullOrWhiteSpace(newTitle))
                throw new ArgumentException("Title cannot be empty");

            if (newText != null && string.IsNullOrWhiteSpace(newText))
                throw new ArgumentException("Text cannot be empty");

            Title = newTitle ?? Title;
            Text = newText ?? Text;
        }

        public void MakeAsRead() => IsRead = true;
        public void Delete() => IsDeleted = true;
    }
}
