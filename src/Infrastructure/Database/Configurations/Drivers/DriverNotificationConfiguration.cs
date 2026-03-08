using Domain.Drivers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.Configurations.Drivers;

internal sealed class DriverNotificationConfiguration : IEntityTypeConfiguration<DriverNotification>
{
    public void Configure(EntityTypeBuilder<DriverNotification> builder)
    {
        builder.ToTable("driver_notifications");

        builder.HasKey(n => n.Id);

        builder.Property(n => n.DriverId).IsRequired();
        builder.HasIndex(n => n.DriverId);

        builder.Property(n => n.NotificationType).IsRequired();
        builder.Property(n => n.Title).IsRequired().HasMaxLength(200);
        builder.Property(n => n.Body).IsRequired().HasMaxLength(1000);
        
        builder.Property(n => n.RelatedEntityId);
        builder.Property(n => n.RelatedEntityType).HasMaxLength(100);
        
        builder.Property(n => n.IsRead).IsRequired().HasDefaultValue(false);
        builder.Property(n => n.ReadAt);
        builder.Property(n => n.SentAt).IsRequired();
        
        builder.Property(n => n.Channel).IsRequired();
    }
}
