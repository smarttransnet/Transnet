using Domain.Assets;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Assets;

internal sealed class AssetLocationConfiguration : IEntityTypeConfiguration<AssetLocation>
{
    public void Configure(EntityTypeBuilder<AssetLocation> builder)
    {
        builder.HasKey(l => l.Id);
        
        builder.Property(l => l.LocationName).HasMaxLength(200);

        builder.Property(l => l.RecordedAt).HasConversion(d => DateTime.SpecifyKind(d, DateTimeKind.Utc), v => v);
    }
}
