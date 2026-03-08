using Domain.Trips;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Trips;

internal sealed class ImportBatchConfiguration : IEntityTypeConfiguration<ImportBatch>
{
    public void Configure(EntityTypeBuilder<ImportBatch> builder)
    {
        builder.HasKey(ib => ib.Id);

        builder.Property(ib => ib.FileName).IsRequired().HasMaxLength(200);
        builder.Property(ib => ib.ErrorSummary).HasMaxLength(2000);

        builder.Property(ib => ib.UploadedAt).HasConversion(d => DateTime.SpecifyKind(d, DateTimeKind.Utc), v => v);

        builder.HasMany(ib => ib.ImportedTrips)
            .WithOne()
            .HasForeignKey(t => t.ImportBatchId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
