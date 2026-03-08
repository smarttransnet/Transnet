using Domain.Trips;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Trips;

internal sealed class TripPodUploadConfiguration : IEntityTypeConfiguration<TripPodUpload>
{
    public void Configure(EntityTypeBuilder<TripPodUpload> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.FileUrl).IsRequired().HasMaxLength(500);
        builder.Property(p => p.FileName).IsRequired().HasMaxLength(200);

        builder.Property(p => p.UploadedAt).HasConversion(d => DateTime.SpecifyKind(d, DateTimeKind.Utc), v => v);
    }
}
