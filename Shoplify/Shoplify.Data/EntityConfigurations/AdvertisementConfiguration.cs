﻿namespace Shoplify.Data.EntityConfigurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Domain;

    public class AdvertisementConfiguration : IEntityTypeConfiguration<Advertisement>
    {
        public void Configure(EntityTypeBuilder<Advertisement> advertisement)
        {
            advertisement
                .HasMany(a => a.Comments)
                .WithOne(c => c.Advertisement)
                .HasForeignKey(c => c.AdvertisementId)
                .OnDelete(DeleteBehavior.Restrict);

            advertisement
                .HasMany(a => a.Reports)
                .WithOne(r => r.ReportedAdvertisement)
                .HasForeignKey(r => r.ReportedAdvertisementId)
                .OnDelete(DeleteBehavior.Restrict);

            advertisement
                .HasOne(a => a.Category)
                .WithMany(c => c.Advertisements)
                .HasForeignKey(a => a.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            advertisement
                .Property(a => a.Price)
                .HasColumnType("decimal");
        }
    }
}
