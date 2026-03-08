using Domain.Billing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.Configurations.Billing;

internal sealed class InvoiceReminderLogConfiguration : IEntityTypeConfiguration<InvoiceReminderLog>
{
    public void Configure(EntityTypeBuilder<InvoiceReminderLog> builder)
    {
        builder.ToTable("InvoiceReminderLogs");
        builder.HasKey(rl => rl.Id);

        builder.Property(rl => rl.SentToEmail).HasMaxLength(500).IsRequired();
        builder.Property(rl => rl.DeliveryError).HasMaxLength(1500);

        builder.HasIndex(rl => rl.InvoiceId);
        builder.HasIndex(rl => rl.SentAt);
    }
}
