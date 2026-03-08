using Domain.Payroll;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.Configurations.Payroll;

internal sealed class DriverSalaryRecordConfiguration : IEntityTypeConfiguration<DriverSalaryRecord>
{
    public void Configure(EntityTypeBuilder<DriverSalaryRecord> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.BaseSalaryQAR).HasPrecision(18, 2);
        builder.Property(x => x.AllowancesQAR).HasPrecision(18, 2);
        builder.Property(x => x.OvertimeQAR).HasPrecision(18, 2);
        builder.Property(x => x.DeductionsQAR).HasPrecision(18, 2);
        builder.Property(x => x.CommissionQAR).HasPrecision(18, 2);
        builder.Property(x => x.NetPayableQAR).HasPrecision(18, 2);
        builder.Property(x => x.Notes).HasMaxLength(1000);

        builder.HasOne(x => x.Driver)
            .WithMany()
            .HasForeignKey(x => x.DriverId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(x => x.CommissionItems)
            .WithOne(c => c.SalaryRecord)
            .HasForeignKey(c => c.DriverSalaryRecordId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.ExpenseLines)
            .WithOne(e => e.SalaryRecord)
            .HasForeignKey(e => e.DriverSalaryRecordId)
            .OnDelete(DeleteBehavior.Cascade);
            
        // One record per driver per month
        builder.HasIndex(x => new { x.DriverId, x.PeriodYear, x.PeriodMonth }).IsUnique();
    }
}
