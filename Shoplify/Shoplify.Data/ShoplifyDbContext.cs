namespace Shoplify.Web.Data
{
    using System.Linq;

    using Domain;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;

    public class ShoplifyDbContext : IdentityDbContext<User>
    {
        public ShoplifyDbContext(DbContextOptions<ShoplifyDbContext> options)
            : base(options)
        {
        }

        public DbSet<Advertisement> Advertisements { get; set; }

        public DbSet<Message> Messages { get; set; }

        public DbSet<Comment> Comments { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Report> Reports { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<Courier> Couriers { get; set; }

        public DbSet<Notification> Notifications { get; set; }

        public DbSet<UserNotification> UsersNotifications { get; set; }

        public DbSet<FollowerFollowing> FollowersFollowings { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            foreach (var relationship in builder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }

            base.OnModelCreating(builder);

            builder
                .Entity<FollowerFollowing>()
                .HasKey(ff => new { ff.FollowingId, ff.FollowerId });

            builder
                .Entity<FollowerFollowing>()
                .HasOne(ff => ff.Following)
                .WithMany(ff => ff.Followings)
                .HasForeignKey(ff => ff.FollowingId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .Entity<FollowerFollowing>()
                .HasOne(ff => ff.Follower)
                .WithMany(ff => ff.Followers)
                .HasForeignKey(ff => ff.FollowerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .Entity<UserNotification>()
                .HasKey(un => new { un.UserId, un.NotificationId });

            builder
                .Entity<UserNotification>()
                .HasOne(un => un.User)
                .WithMany(un => un.Notifications)
                .HasForeignKey(un => un.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .Entity<UserNotification>()
                .HasOne(un => un.Notification)
                .WithMany(un => un.Users)
                .HasForeignKey(un => un.NotificationId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .Entity<User>()
                .HasMany(u => u.Advertisements)
                .WithOne(a => a.User)
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .Entity<User>()
                .HasMany(u => u.Messages)
                .WithOne(m => m.Sender)
                .HasForeignKey(m => m.SenderId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .Entity<User>()
                .HasMany(u => u.Orders)
                .WithOne(o => o.Buyer)
                .HasForeignKey(o => o.BuyerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .Entity<User>()
                .HasMany(u => u.Comments)
                .WithOne(c => c.User)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .Entity<Advertisement>()
                .HasMany(a => a.Comments)
                .WithOne(c => c.Advertisement)
                .HasForeignKey(c => c.AdvertisementId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .Entity<Advertisement>()
                .HasMany(a => a.Reports)
                .WithOne(r => r.ReportedAdvertisement)
                .HasForeignKey(r => r.ReportedAdvertisementId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .Entity<Advertisement>()
                .HasOne(a => a.Category)
                .WithMany(c => c.Advertisements)
                .HasForeignKey(a => a.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .Entity<Advertisement>()
                .Property(a => a.Price)
                .HasColumnType("decimal");

            builder
                .Entity<Order>()
                .Property(a => a.Price)
                .HasColumnType("decimal");

            builder
                .Entity<Courier>()
                .HasMany(c => c.Orders)
                .WithOne(o => o.Courier)
                .HasForeignKey(o => o.CourierId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
