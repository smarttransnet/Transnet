using Domain.Payroll;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.Configurations.Payroll;

internal sealed class SalaryExpenseLineConfiguration : IEntityTypeConfiguration<SalaryExpenseLine>
{
    public void Configure(EntityTypeBuilder<SalaryExpenseLine> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Description).IsRequired().HasMaxLength(255);
        builder.Property(x => x.AmountQAR).HasPrecision(18, 2);

        builder.HasOne(x => x.DriverExpense)
            .WithMany()
            .HasForeignKey(x => x.DriverExpenseId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
