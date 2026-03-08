using Domain.Inspections;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Inspections;

internal sealed class ChecklistItemConfiguration : IEntityTypeConfiguration<ChecklistItem>
{
    public void Configure(EntityTypeBuilder<ChecklistItem> builder)
    {
        builder.HasKey(i => i.Id);
        
        builder.Property(i => i.ItemName).IsRequired().HasMaxLength(200);
        builder.Property(i => i.Category).HasMaxLength(100);

        builder.HasOne(i => i.InspectionChecklist)
            .WithMany(c => c.Items)
            .HasForeignKey(i => i.InspectionChecklistId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
