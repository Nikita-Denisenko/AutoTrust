namespace AutoTrust.Domain.Entities
{
    public class Chat
    {
        public int Id { get; private set; }
        private readonly List<ChatParticipant> _chatParticipants = [];
        public IReadOnlyCollection<ChatParticipant> ChatParticipants => _chatParticipants;
        public ICollection<Message> Messages { get; private set; } = [];
        public DateTime CreatedAt { get; private set; }
        public Message? PinnedMessage { get; private set; }

        private Chat() { }

        public Chat(int user1Id, int user2Id)
        {
            if (user1Id == user2Id)
                throw new InvalidOperationException("Cannot create chat with yourself");

            _chatParticipants.Add(new ChatParticipant(user1Id));
            _chatParticipants.Add(new ChatParticipant(user2Id));
            CreatedAt = DateTime.UtcNow;
        }

        public void PinMessage(Message message) => PinnedMessage = message;

        public void UnpinCurrentMessage() => PinnedMessage = null;
    }
}
