using Domain.Clients;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.Configurations.Clients;

internal sealed class ClientConfiguration : IEntityTypeConfiguration<Client>
{
    public void Configure(EntityTypeBuilder<Client> builder)
    {
        builder.ToTable("Clients");
        builder.HasKey(c => c.Id);

        builder.Property(c => c.ClientCode).HasMaxLength(50).IsRequired();
        builder.Property(c => c.CompanyName).HasMaxLength(200).IsRequired();
        builder.Property(c => c.ContactPersonName).HasMaxLength(200).IsRequired();
        builder.Property(c => c.ContactEmail).HasMaxLength(200).IsRequired();
        builder.Property(c => c.ContactPhone).HasMaxLength(50).IsRequired();
        builder.Property(c => c.BillingAddress).HasMaxLength(500).IsRequired();
        builder.Property(c => c.TaxRegistrationNumber).HasMaxLength(100);
        builder.Property(c => c.CurrencyCode).HasMaxLength(10).IsRequired();

        builder.HasIndex(c => c.ClientCode).IsUnique();
        builder.HasIndex(c => c.CompanyName).IsUnique();

        builder.HasMany(c => c.PortalUsers)
            .WithOne(p => p.Client)
            .HasForeignKey(p => p.ClientId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(c => c.Quotations)
            .WithOne(q => q.Client)
            .HasForeignKey(q => q.ClientId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(c => c.Invoices)
            .WithOne(i => i.Client)
            .HasForeignKey(i => i.ClientId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(c => c.OutstandingReports)
            .WithOne(r => r.Client)
            .HasForeignKey(r => r.ClientId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
