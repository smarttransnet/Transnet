using Domain.Trips;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Trips;

internal sealed class TripVoucherConfiguration : IEntityTypeConfiguration<TripVoucher>
{
    public void Configure(EntityTypeBuilder<TripVoucher> builder)
    {
        builder.HasKey(v => v.Id);

        builder.Property(v => v.VoucherNumber).IsRequired().HasMaxLength(50);
        builder.Property(v => v.Notes).HasMaxLength(1000);

        builder.Property(v => v.VoucherDate).HasConversion(d => DateTime.SpecifyKind(d, DateTimeKind.Utc), v => v);
        builder.Property(v => v.CreatedAt).HasConversion(d => DateTime.SpecifyKind(d, DateTimeKind.Utc), v => v);

        builder.HasMany(v => v.CustomFields)
            .WithOne()
            .HasForeignKey(cf => cf.TripVoucherId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
