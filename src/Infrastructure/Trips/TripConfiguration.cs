using Domain.Trips;
using Domain.Clients;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Trips;

internal sealed class TripConfiguration : IEntityTypeConfiguration<Trip>
{
    public void Configure(EntityTypeBuilder<Trip> builder)
    {
        builder.HasKey(t => t.Id);

        builder.Property(t => t.TripNumber).IsRequired().HasMaxLength(50);
        builder.Property(t => t.Origin).IsRequired().HasMaxLength(200);
        builder.Property(t => t.Destination).IsRequired().HasMaxLength(200);
        builder.Property(t => t.TotalDistanceKm).HasPrecision(18, 2);

        builder.HasIndex(t => t.ClientId);

        builder.HasOne(t => t.Client)
            .WithMany()
            .HasForeignKey(t => t.ClientId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(t => t.ScheduledStartAt).HasConversion(d => DateTime.SpecifyKind(d, DateTimeKind.Utc), v => v);
        builder.Property(t => t.ActualStartAt).HasConversion(d => d.HasValue ? DateTime.SpecifyKind(d.Value, DateTimeKind.Utc) : d, v => v);
        builder.Property(t => t.ActualEndAt).HasConversion(d => d.HasValue ? DateTime.SpecifyKind(d.Value, DateTimeKind.Utc) : d, v => v);
        builder.Property(t => t.DriverConfirmedAt).HasConversion(d => d.HasValue ? DateTime.SpecifyKind(d.Value, DateTimeKind.Utc) : d, v => v);
        builder.Property(t => t.OfficeApprovedAt).HasConversion(d => d.HasValue ? DateTime.SpecifyKind(d.Value, DateTimeKind.Utc) : d, v => v);
        builder.Property(t => t.CreatedAt).HasConversion(d => DateTime.SpecifyKind(d, DateTimeKind.Utc), v => v);
        builder.Property(t => t.UpdatedAt).HasConversion(d => DateTime.SpecifyKind(d, DateTimeKind.Utc), v => v);



        builder.HasMany(t => t.StatusHistory)
            .WithOne(sh => sh.Trip)
            .HasForeignKey(sh => sh.TripId)
            .OnDelete(DeleteBehavior.Cascade);



        builder.Property(t => t.VehicleCategoryUomId);
        builder.Property(t => t.Quantity).HasPrecision(18, 2);

        builder.HasOne(t => t.VehicleCategoryUom)
            .WithMany()
            .HasForeignKey(t => t.VehicleCategoryUomId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
