namespace AutoTrust.Domain.Entities
{
    public class Message
    {
        public int Id { get; private set; }
        public string Text { get; private set; }
        public int ChatId { get; private set; }
        public Chat Chat { get; private set; }
        public int ChatParticipantId { get; private set; }
        public ChatParticipant ChatParticipant { get; private set; }
        public bool IsRead { get; private set; } = false;
        public bool IsDeleted { get; private set; } = false;
        public DateTime SentAt { get; private set; }

        private Message() { }

        public Message
        (
            string text,
            int chatId,
            int chatParticipantId
        )
        {
            if (string.IsNullOrWhiteSpace(text))
                throw new ArgumentException("Message cannot be empty!");

            if (chatId <= 0)
                throw new ArgumentException("ChatId must be positive!");

            if (chatParticipantId <= 0)
                throw new ArgumentException("ChatParticipantId must be positive!");

            Text = text; 
            ChatId = chatId; 
            ChatParticipantId = chatParticipantId; 
            SentAt = DateTime.UtcNow;
        }

        public void Update(string newText)
        {
            if (string.IsNullOrWhiteSpace(newText))
                throw new ArgumentException("Message cannot be empty!");

            Text = newText;
        }

        public void Delete() => IsDeleted = true;
        public void Read() => IsRead = true;
    }
}
