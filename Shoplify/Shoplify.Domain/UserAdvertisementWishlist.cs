namespace Shoplify.Domain
{
    public class UserAdvertisementWishlist
    {
        public string UserId { get; set; }

        public User User { get; set; }

        public string AdvertisementId { get; set; }

        public Advertisement Advertisement { get; set; }
    }
}
