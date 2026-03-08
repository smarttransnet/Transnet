using Domain.Assets;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Assets;

internal sealed class TrailerConfiguration : IEntityTypeConfiguration<Trailer>
{
    public void Configure(EntityTypeBuilder<Trailer> builder)
    {
        builder.HasKey(t => t.Id);
        
        builder.Property(t => t.TrailerNumber).IsRequired().HasMaxLength(50);
        builder.Property(t => t.TrailerType).IsRequired().HasMaxLength(100);
        builder.Property(t => t.CapacityUnit).IsRequired().HasMaxLength(20);
        
        builder.Property(t => t.CreatedAt).HasConversion(d => DateTime.SpecifyKind(d, DateTimeKind.Utc), v => v);
        builder.Property(t => t.UpdatedAt).HasConversion(d => DateTime.SpecifyKind(d, DateTimeKind.Utc), v => v);

        builder.HasOne(t => t.AttachedVehicle)
            .WithMany()
            .HasForeignKey(t => t.AttachedVehicleId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
