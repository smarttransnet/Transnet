using Domain.Fuel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.Configurations.Fuel;

internal sealed class WoqoodImportBatchConfiguration : IEntityTypeConfiguration<WoqoodImportBatch>
{
    public void Configure(EntityTypeBuilder<WoqoodImportBatch> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.FileName).IsRequired().HasMaxLength(255);
        builder.Property(x => x.TotalLitres).HasPrecision(18, 2);
        builder.Property(x => x.TotalAmountQAR).HasPrecision(18, 2);

        builder.HasMany(x => x.Transactions)
            .WithOne(t => t.ImportBatch)
            .HasForeignKey(t => t.WoqoodImportBatchId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
