using System;
using System.Reflection;

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
            base.OnModelCreating(builder);

            foreach (var relationship in builder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }

            builder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        }
    }
}
