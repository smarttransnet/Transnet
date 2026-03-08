using Domain.Trips;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Trips;

internal sealed class TripHaltConfiguration : IEntityTypeConfiguration<TripHalt>
{
    public void Configure(EntityTypeBuilder<TripHalt> builder)
    {
        builder.HasKey(h => h.Id);

        builder.Property(h => h.Reason).HasMaxLength(500);
        builder.Property(h => h.LocationName).HasMaxLength(200);

        builder.Property(h => h.StartedAt).HasConversion(d => DateTime.SpecifyKind(d, DateTimeKind.Utc), v => v);
        builder.Property(h => h.EndedAt).HasConversion(d => d.HasValue ? DateTime.SpecifyKind(d.Value, DateTimeKind.Utc) : d, v => v);
    }
}
