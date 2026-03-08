using Domain.Inspections;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Inspections;

internal sealed class InspectionResultConfiguration : IEntityTypeConfiguration<InspectionResult>
{
    public void Configure(EntityTypeBuilder<InspectionResult> builder)
    {
        builder.HasKey(r => r.Id);
        
        builder.Property(r => r.Remarks).HasMaxLength(500);

        builder.Property(r => r.RecordedAt).HasConversion(d => DateTime.SpecifyKind(d, DateTimeKind.Utc), v => v);

        builder.HasOne(r => r.VehicleInspection)
            .WithMany(i => i.InspectionResults)
            .HasForeignKey(r => r.VehicleInspectionId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(r => r.ChecklistItem)
            .WithMany()
            .HasForeignKey(r => r.ChecklistItemId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
