using Domain.Fuel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.Configurations.Fuel;

internal sealed class VehicleFuelSummaryConfiguration : IEntityTypeConfiguration<VehicleFuelSummary>
{
    public void Configure(EntityTypeBuilder<VehicleFuelSummary> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.TotalLitres).HasPrecision(18, 2);
        builder.Property(x => x.TotalCostQAR).HasPrecision(18, 2);
        builder.Property(x => x.AverageCostPerLitreQAR).HasPrecision(18, 2);
        builder.Property(x => x.AverageFuelEfficiencyKmPerL).HasPrecision(18, 2);

        builder.HasOne(x => x.Vehicle)
            .WithMany()
            .HasForeignKey(x => x.VehicleId)
            .OnDelete(DeleteBehavior.Cascade);

        // Ensures one summary per vehicle per month
        builder.HasIndex(x => new { x.VehicleId, x.PeriodYear, x.PeriodMonth }).IsUnique();
    }
}
