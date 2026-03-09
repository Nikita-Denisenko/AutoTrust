namespace AutoTrust.Domain.Entities
{
    public class Chat
    {
        public int Id { get; private set; } 
        public ICollection<ChatParticipant> ChatParticipants { get; private set; } = [];
        public DateTime CreatedAt { get; private set; }
        public int? PinnedMessageId { get; private set; }

        public Chat() 
        {
            CreatedAt = DateTime.UtcNow;
        }

        public void PinMessage(int messageId)
        {
            if (messageId <= 0)
                throw new ArgumentException("MessageId must be positive!");

            PinnedMessageId = messageId;
        } 

        public void UnpinCurrentMessage() => PinnedMessageId = null;
    }
}
