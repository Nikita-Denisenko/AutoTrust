namespace AutoTrust.Domain.Entities
{
    public class Chat
    {
        public int Id { get; private set; } 
        public string Name { get; private set; }
        public ICollection<ChatParticipant> ChatParticipants { get; private set; } = [];
        public DateTime CreatedAt { get; private set; }

        private Chat() { }

        public Chat(string name) 
        {
            Name = name;
            CreatedAt = DateTime.UtcNow;
        }
    }
}
