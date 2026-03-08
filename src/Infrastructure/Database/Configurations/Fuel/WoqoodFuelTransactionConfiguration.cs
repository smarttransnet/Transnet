using Domain.Fuel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.Configurations.Fuel;

internal sealed class WoqoodFuelTransactionConfiguration : IEntityTypeConfiguration<WoqoodFuelTransaction>
{
    public void Configure(EntityTypeBuilder<WoqoodFuelTransaction> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.WoqoodCardNumber).IsRequired().HasMaxLength(50);
        builder.Property(x => x.StationName).IsRequired().HasMaxLength(255);
        builder.Property(x => x.QuantityLitres).HasPrecision(18, 2);
        builder.Property(x => x.UnitPriceQAR).HasPrecision(18, 2);
        builder.Property(x => x.TotalAmountQAR).HasPrecision(18, 2);
        builder.Property(x => x.Odometer).HasPrecision(18, 2);

        builder.HasOne(x => x.Allocation)
            .WithOne(a => a.WoqoodTransaction)
            .HasForeignKey<FuelCostAllocation>(a => a.WoqoodFuelTransactionId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
