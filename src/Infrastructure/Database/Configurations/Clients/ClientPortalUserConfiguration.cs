using Domain.Clients;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.Configurations.Clients;

internal sealed class ClientPortalUserConfiguration : IEntityTypeConfiguration<ClientPortalUser>
{
    public void Configure(EntityTypeBuilder<ClientPortalUser> builder)
    {
        builder.ToTable("ClientPortalUsers");
        builder.HasKey(u => u.Id);

        builder.Property(u => u.FullName).HasMaxLength(200).IsRequired();
        builder.Property(u => u.Email).HasMaxLength(200).IsRequired();
        builder.Property(u => u.PasswordHash).HasMaxLength(500).IsRequired();
        builder.Property(u => u.PasswordSalt).HasMaxLength(500).IsRequired();
        builder.Property(u => u.RefreshToken).HasMaxLength(500);

        builder.HasIndex(u => u.Email).IsUnique();
        builder.HasIndex(u => u.ClientId);
    }
}
