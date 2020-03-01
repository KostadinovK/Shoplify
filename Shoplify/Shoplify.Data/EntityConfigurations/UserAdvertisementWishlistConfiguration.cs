namespace Shoplify.Data.EntityConfigurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Shoplify.Domain;

    public class UserAdvertisementWishlistConfiguration : IEntityTypeConfiguration<UserAdvertisementWishlist>
    {
        public void Configure(EntityTypeBuilder<UserAdvertisementWishlist> entity)
        {
            entity
                .HasKey(ua => new { ua.UserId, ua.AdvertisementId });

            entity
                .HasOne(ua => ua.User)
                .WithMany(u => u.WishList)
                .HasForeignKey(ua => ua.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            entity
                .HasOne(ua => ua.Advertisement)
                .WithMany(a => a.Users)
                .HasForeignKey(ua => ua.AdvertisementId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
