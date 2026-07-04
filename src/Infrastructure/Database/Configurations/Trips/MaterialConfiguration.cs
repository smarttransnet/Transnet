using Domain.Trips;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.Configurations.Trips;

internal sealed class MaterialConfiguration : IEntityTypeConfiguration<Material>
{
    public void Configure(EntityTypeBuilder<Material> builder)
    {
        builder.ToTable("materials");
        builder.HasKey(m => m.Id);
        builder.Property(m => m.MaterialName).IsRequired().HasMaxLength(150);
        builder.Property(m => m.IsActive).IsRequired().HasDefaultValue(true);
        builder.Property(m => m.CreatedDate).IsRequired();
        builder.Property(m => m.CreatedBy).IsRequired();

    }
}
