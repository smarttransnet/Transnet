using Domain.Reports;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.Configurations.Reports;

internal sealed class ExpenseReportLineItemConfiguration : IEntityTypeConfiguration<ExpenseReportLineItem>
{
    public void Configure(EntityTypeBuilder<ExpenseReportLineItem> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.SubCategory).HasMaxLength(100);
        builder.Property(x => x.EntityType).HasMaxLength(100);
        builder.Property(x => x.Description).IsRequired().HasMaxLength(255);
        builder.Property(x => x.AmountQAR).HasPrecision(18, 2);
    }
}
