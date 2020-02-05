namespace Shoplify.Domain
{
    public class FollowerFollowing
    {
        public string FollowingId { get; set; }

        public User Following { get; set; }

        public string FollowerId { get; set; }

        public User Follower { get; set; }
    }
}
