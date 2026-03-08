using Domain.Trips;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Trips;

internal sealed class TripCustomFieldConfiguration : IEntityTypeConfiguration<TripCustomField>
{
    public void Configure(EntityTypeBuilder<TripCustomField> builder)
    {
        builder.HasKey(tcf => tcf.Id);

        builder.Property(tcf => tcf.Value).IsRequired().HasMaxLength(1000);

        builder.HasOne(tcf => tcf.FieldDefinition)
            .WithMany()
            .HasForeignKey(tcf => tcf.FieldDefinitionId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
