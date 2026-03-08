using Domain.Fuel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.Configurations.Fuel;

internal sealed class WoqoodCardMappingConfiguration : IEntityTypeConfiguration<WoqoodCardMapping>
{
    public void Configure(EntityTypeBuilder<WoqoodCardMapping> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.WoqoodCardNumber).IsRequired().HasMaxLength(50);
        builder.Property(x => x.CardHolderName).IsRequired().HasMaxLength(150);
        builder.Property(x => x.Notes).HasMaxLength(500);

        builder.HasIndex(x => x.WoqoodCardNumber).IsUnique();

        builder.HasOne(x => x.Vehicle)
            .WithMany()
            .HasForeignKey(x => x.VehicleId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(x => x.Driver)
            .WithMany()
            .HasForeignKey(x => x.DriverId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
