using Domain.Inspections;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Inspections;

internal sealed class InspectionPhotoConfiguration : IEntityTypeConfiguration<InspectionPhoto>
{
    public void Configure(EntityTypeBuilder<InspectionPhoto> builder)
    {
        builder.HasKey(p => p.Id);
        
        builder.Property(p => p.PhotoPath).IsRequired().HasMaxLength(500);
        builder.Property(p => p.Caption).HasMaxLength(200);

        builder.Property(p => p.UploadedAt).HasConversion(d => DateTime.SpecifyKind(d, DateTimeKind.Utc), v => v);

        builder.HasOne(p => p.VehicleInspection)
            .WithMany(i => i.Photos)
            .HasForeignKey(p => p.VehicleInspectionId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
