namespace Shoplify.Data.EntityConfigurations
{
    using Domain;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class FollowerFollowingConfiguration : IEntityTypeConfiguration<FollowerFollowing>
    {
        public void Configure(EntityTypeBuilder<FollowerFollowing> followerFollowing)
        {
            followerFollowing
                .HasKey(ff => new { ff.FollowingId, ff.FollowerId });

            followerFollowing
                .HasOne(ff => ff.Following)
                .WithMany(ff => ff.Followings)
                .HasForeignKey(ff => ff.FollowingId)
                .OnDelete(DeleteBehavior.Restrict);

            followerFollowing
                .HasOne(ff => ff.Follower)
                .WithMany(ff => ff.Followers)
                .HasForeignKey(ff => ff.FollowerId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
