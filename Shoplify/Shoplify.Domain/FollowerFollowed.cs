namespace Shoplify.Domain
{
    public class FollowerFollowed
    {
        public string FollowerId { get; set; }

        public string FollowedId { get; set; }

        public User Follower { get; set; }

        public User Followed { get; set; }
    }
}
