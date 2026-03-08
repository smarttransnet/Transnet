using Domain.Trips;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Trips;

internal sealed class TripStatusHistoryConfiguration : IEntityTypeConfiguration<TripStatusHistory>
{
    public void Configure(EntityTypeBuilder<TripStatusHistory> builder)
    {
        builder.HasKey(sh => sh.Id);

        builder.Property(sh => sh.Notes).HasMaxLength(1000);

        builder.Property(sh => sh.ChangedAt).HasConversion(d => DateTime.SpecifyKind(d, DateTimeKind.Utc), v => v);
    }
}
