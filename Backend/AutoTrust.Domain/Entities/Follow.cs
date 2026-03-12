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
        public bool IsDeleted { get; private set; } = false;

        private Follow() { }

        public Follow
        (
            int followerId,
            int targetId
        ) 
        {
            if (followerId <= 0)
                throw new ArgumentOutOfRangeException(nameof(followerId), followerId, "FollowerId must be positive!");

            if (targetId <= 0)
                throw new ArgumentOutOfRangeException(nameof(targetId), targetId, "TargetId must be positive!");

            FollowerId = followerId;
            TargetId = targetId;
            FollowedAt = DateTime.UtcNow;
        }

        public void Delete() => IsDeleted = true;
    }
}
