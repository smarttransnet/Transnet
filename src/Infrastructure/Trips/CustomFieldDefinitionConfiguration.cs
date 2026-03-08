using Domain.Trips;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Trips;

internal sealed class CustomFieldDefinitionConfiguration : IEntityTypeConfiguration<CustomFieldDefinition>
{
    public void Configure(EntityTypeBuilder<CustomFieldDefinition> builder)
    {
        builder.HasKey(cfd => cfd.Id);

        builder.Property(cfd => cfd.FieldName).IsRequired().HasMaxLength(100);
        builder.Property(cfd => cfd.FieldLabel).IsRequired().HasMaxLength(100);
        builder.Property(cfd => cfd.DataType).IsRequired().HasMaxLength(50);
        builder.Property(cfd => cfd.AppliesTo).IsRequired().HasMaxLength(50);
    }
}
