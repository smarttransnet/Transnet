using Domain.Drivers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.Configurations.Drivers;

internal sealed class DriverExpenseConfiguration : IEntityTypeConfiguration<DriverExpense>
{
    public void Configure(EntityTypeBuilder<DriverExpense> builder)
    {
        builder.ToTable("driver_expenses");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.DriverId).IsRequired();
        builder.HasIndex(e => e.DriverId);

        // Optional FK to Trip
        builder.Property(e => e.TripId);
        builder.HasIndex(e => e.TripId); // useful for fetching all expenses for a trip

        builder.Property(e => e.ExpenseType).IsRequired();
        builder.Property(e => e.AmountQAR).IsRequired().HasPrecision(18, 2);
        builder.Property(e => e.ExpenseDate).IsRequired();
        
        builder.Property(e => e.Description).HasMaxLength(1000);
        builder.Property(e => e.ReceiptUrl).HasMaxLength(2048);
        
        // Fuel specific
        builder.Property(e => e.FuelLitres).HasPrecision(10, 2);
        builder.Property(e => e.FuelStation).HasMaxLength(200);
        builder.Property(e => e.OdometerReading).HasPrecision(18, 2);
        
        builder.Property(e => e.Status).IsRequired();
        builder.Property(e => e.ReviewedByUserId);
        builder.Property(e => e.ReviewedAt);
        builder.Property(e => e.SubmittedAt).IsRequired();
    }
}
