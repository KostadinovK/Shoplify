namespace Shoplify.Data.EntityConfigurations
{
    using System;

    using Domain;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class UserNotificationConfiguration : IEntityTypeConfiguration<UserNotification>
    {
        public void Configure(EntityTypeBuilder<UserNotification> userNotification)
        {
            userNotification
                .HasKey(un => new { un.UserId, un.NotificationId });

            userNotification
                .HasOne(un => un.User)
                .WithMany(un => un.Notifications)
                .HasForeignKey(un => un.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            userNotification
                .HasOne(un => un.Notification)
                .WithMany(un => un.Users)
                .HasForeignKey(un => un.NotificationId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
