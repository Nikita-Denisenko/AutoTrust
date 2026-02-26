using System.Runtime.InteropServices;

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
    }
}
