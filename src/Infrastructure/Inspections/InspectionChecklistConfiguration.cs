using Domain.Inspections;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Inspections;

internal sealed class InspectionChecklistConfiguration : IEntityTypeConfiguration<InspectionChecklist>
{
    public void Configure(EntityTypeBuilder<InspectionChecklist> builder)
    {
        builder.HasKey(c => c.Id);
        
        builder.Property(c => c.Name).IsRequired().HasMaxLength(200);
        builder.Property(c => c.ApplicableVehicleTypes).HasMaxLength(500);

        builder.Property(c => c.CreatedAt).HasConversion(d => DateTime.SpecifyKind(d, DateTimeKind.Utc), v => v);

        // One-to-many relationship is configured by EF convention or from the ChecklistItem side.
    }
}
