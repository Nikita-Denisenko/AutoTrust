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
        public DateTime SentAt { get; private set; }

        private Message() { }

        public Message
        (
            string text,
            int chatId,
            int chatParticipantId,
            bool isRead
        )
        {
            Text = text; 
            ChatId = chatId; 
            ChatParticipantId = chatParticipantId; 
            IsRead = isRead;
            SentAt = DateTime.UtcNow;
        }
    }
}
