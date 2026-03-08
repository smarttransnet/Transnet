using Domain.Payroll;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.Configurations.Payroll;

internal sealed class DriverCommissionItemConfiguration : IEntityTypeConfiguration<DriverCommissionItem>
{
    public void Configure(EntityTypeBuilder<DriverCommissionItem> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Description).IsRequired().HasMaxLength(255);
        builder.Property(x => x.AmountQAR).HasPrecision(18, 2);
        builder.Property(x => x.CalculationBasis).HasMaxLength(500);

        builder.HasOne(x => x.Trip)
            .WithMany()
            .HasForeignKey(x => x.TripId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
