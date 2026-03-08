using Domain.Inspections;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Inspections;

internal sealed class VehicleInspectionConfiguration : IEntityTypeConfiguration<VehicleInspection>
{
    public void Configure(EntityTypeBuilder<VehicleInspection> builder)
    {
        builder.HasKey(i => i.Id);
        
        builder.Property(i => i.DriverSignature).HasMaxLength(200);
        builder.Property(i => i.Notes).HasMaxLength(1000);

        builder.Property(i => i.InspectedAt).HasConversion(d => DateTime.SpecifyKind(d, DateTimeKind.Utc), v => v);
        builder.Property(i => i.DriverSignedAt).HasConversion(d => d.HasValue ? DateTime.SpecifyKind(d.Value, DateTimeKind.Utc) : d, v => v);

        builder.HasOne(i => i.Vehicle)
            .WithMany()
            .HasForeignKey(i => i.VehicleId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(i => i.InspectionChecklist)
            .WithMany()
            .HasForeignKey(i => i.InspectionChecklistId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
