using Domain.Billing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.Configurations.Billing;

internal sealed class InvoiceTripLinkConfiguration : IEntityTypeConfiguration<InvoiceTripLink>
{
    public void Configure(EntityTypeBuilder<InvoiceTripLink> builder)
    {
        builder.ToTable("InvoiceTripLinks");
        builder.HasKey(tl => tl.Id);

        builder.HasIndex(tl => tl.InvoiceId);
        builder.HasIndex(tl => tl.TripId);
        builder.HasIndex(tl => new { tl.InvoiceId, tl.TripId }).IsUnique();

        builder.HasOne(tl => tl.Trip)
            .WithMany()
            .HasForeignKey(tl => tl.TripId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
