using Domain.Drivers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.Configurations.Drivers;

internal sealed class DriverAuthCredentialConfiguration : IEntityTypeConfiguration<DriverAuthCredential>
{
    public void Configure(EntityTypeBuilder<DriverAuthCredential> builder)
    {
        builder.ToTable("driver_auth_credentials");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.DriverId).IsRequired();
        builder.HasIndex(c => c.DriverId).IsUnique(); // Enforce One-to-One

        builder.Property(c => c.UsernameHash).IsRequired().HasMaxLength(512);
        builder.Property(c => c.PasswordHash).IsRequired().HasMaxLength(512);
        
        builder.Property(c => c.RefreshToken).HasMaxLength(512);
        builder.Property(c => c.RefreshTokenExpiresAt);
        
        builder.Property(c => c.LastLoginAt);
        builder.Property(c => c.DeviceToken).HasMaxLength(512);
        builder.Property(c => c.Platform).IsRequired();
        
        builder.Property(c => c.IsLocked).IsRequired().HasDefaultValue(false);
        builder.Property(c => c.FailedAttempts).IsRequired().HasDefaultValue(0);
        builder.Property(c => c.UpdatedAt).IsRequired();
    }
}
