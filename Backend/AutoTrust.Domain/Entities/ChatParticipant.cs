namespace AutoTrust.Domain.Entities
{
    public class ChatParticipant
    {
        public int Id { get; private set; }
        public int UserId { get; private set; }
        public User User { get; private set; }
        public int ChatId { get; private set; }
        public Chat Chat { get; private set; }

        public string? UserAvatarUrl { get; private set; }
        public string UserName { get; private set; }

        private ChatParticipant() { }

        public ChatParticipant
        (
            int chatId,
            User user
        )
        {
            UserId = user.Id;
            ChatId = chatId;
            UserAvatarUrl = user.AvatarUrl;
            UserName = user.Name;
        }
    }
}