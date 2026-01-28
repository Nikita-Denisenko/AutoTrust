namespace AutoTrust.Domain.Entities
{
    public class Follow
    {
        public int Id { get; private set; }
        public int FollowerId { get; private set; }
        public User Follower {  get; private set; }
        public int UserId { get; private set; }
        public User User { get; private set; }
        public DateTime FollowedAt { get; private set; }

        private Follow() { }

        public Follow
        (
            int followerId,
            int userId
        ) 
        { 
            FollowerId = followerId;
            UserId = userId;
            FollowedAt = DateTime.UtcNow;
        }
    }
}
