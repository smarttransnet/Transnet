using Domain.Drivers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.Configurations.Drivers;

internal sealed class DriverDocumentConfiguration : IEntityTypeConfiguration<DriverDocument>
{
    public void Configure(EntityTypeBuilder<DriverDocument> builder)
    {
        builder.ToTable("driver_documents");

        builder.HasKey(d => d.Id);

        builder.Property(d => d.DriverId).IsRequired();
        builder.HasIndex(d => d.DriverId);

        // Optional Trip link
        builder.Property(d => d.TripId);
        
        builder.Property(d => d.DocumentType).IsRequired();
        builder.Property(d => d.Title).IsRequired().HasMaxLength(200);
        builder.Property(d => d.FileUrl).IsRequired().HasMaxLength(2048);
        
        builder.Property(d => d.UploadedAt).IsRequired();
        builder.Property(d => d.SubmittedFromApp).IsRequired().HasDefaultValue(true);
        
        builder.Property(d => d.Latitude);
        builder.Property(d => d.Longitude);
    }
}
