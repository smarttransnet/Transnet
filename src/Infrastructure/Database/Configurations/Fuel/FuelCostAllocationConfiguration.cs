using Domain.Fuel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.Configurations.Fuel;

internal sealed class FuelCostAllocationConfiguration : IEntityTypeConfiguration<FuelCostAllocation>
{
    public void Configure(EntityTypeBuilder<FuelCostAllocation> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.QuantityLitres).HasPrecision(18, 2);
        builder.Property(x => x.AmountQAR).HasPrecision(18, 2);
        builder.Property(x => x.Notes).HasMaxLength(500);

        builder.HasOne(x => x.Vehicle)
            .WithMany()
            .HasForeignKey(x => x.VehicleId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.DriverExpense)
            .WithMany()
            .HasForeignKey(x => x.DriverExpenseId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(x => x.Trip)
            .WithMany()
            .HasForeignKey(x => x.TripId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
