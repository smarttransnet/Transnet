using Domain.Trips;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Trips;

internal sealed class TripStopConfiguration : IEntityTypeConfiguration<TripStop>
{
    public void Configure(EntityTypeBuilder<TripStop> builder)
    {
        builder.HasKey(s => s.Id);

        builder.Property(s => s.LocationName).IsRequired().HasMaxLength(200);
        builder.Property(s => s.Address).HasMaxLength(500);
        builder.Property(s => s.PocName).HasMaxLength(100);
        builder.Property(s => s.PocPhone).HasMaxLength(20);
        builder.Property(s => s.PocEmail).HasMaxLength(100);
        builder.Property(s => s.Notes).HasMaxLength(1000);

        builder.Property(s => s.ScheduledArrivalAt).HasConversion(d => d.HasValue ? DateTime.SpecifyKind(d.Value, DateTimeKind.Utc) : d, v => v);
        builder.Property(s => s.ActualArrivalAt).HasConversion(d => d.HasValue ? DateTime.SpecifyKind(d.Value, DateTimeKind.Utc) : d, v => v);
        builder.Property(s => s.ActualDepartureAt).HasConversion(d => d.HasValue ? DateTime.SpecifyKind(d.Value, DateTimeKind.Utc) : d, v => v);

        builder.HasMany(s => s.PodUploads)
            .WithOne(p => p.TripStop)
            .HasForeignKey(p => p.TripStopId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
