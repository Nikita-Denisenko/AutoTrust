namespace AutoTrust.Domain.Entities
{
    public class Follow
    {
        public int Id { get; private set; }
        public int FollowerId { get; private set; }
        public User Follower {  get; private set; }
        public int TargetId { get; private set; }
        public User Target { get; private set; }
        public DateTime FollowedAt { get; private set; }

        private Follow() { }

        public Follow
        (
            int followerId,
            int targetId
        ) 
        { 
            FollowerId = followerId;
            TargetId = targetId;
            FollowedAt = DateTime.UtcNow;
        }
    }
}
