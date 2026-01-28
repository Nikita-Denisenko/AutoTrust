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

        private Notification() { }

        public Notification
        (
            string title, 
            string text, 
            int userId, 
            bool isRead
        )
        {
            Title = title;
            Text = text;
            UserId = userId;
            IsRead = isRead;
            CreatedAt = DateTime.UtcNow;
        }
    }
}
