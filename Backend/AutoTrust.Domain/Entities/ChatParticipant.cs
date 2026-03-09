namespace AutoTrust.Domain.Entities
{
    public class ChatParticipant
    {
        public int Id { get; private set; }
        public int UserId { get; private set; }
        public User User { get; private set; }
        public int ChatId { get; private set; }
        public Chat Chat { get; private set; }

        private ChatParticipant() { }

        public ChatParticipant
        (
            int userId,
            int chatId
        )
        {
            if (userId <= 0)
                throw new ArgumentException("UserId must be positive!");

            if (chatId <= 0)
                throw new ArgumentException("ChatId must be positive!");

            UserId = userId;
            ChatId = chatId;
        }
    }
}