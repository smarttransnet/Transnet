using Domain.Drivers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.Configurations.Drivers;

internal sealed class DriverConfiguration : IEntityTypeConfiguration<Driver>
{
    public void Configure(EntityTypeBuilder<Driver> builder)
    {
        builder.ToTable("drivers");

        builder.HasKey(d => d.Id);

        builder.Property(d => d.EmployeeNumber).IsRequired().HasMaxLength(50);
        builder.HasIndex(d => d.EmployeeNumber).IsUnique();

        builder.Property(d => d.FirstName).IsRequired().HasMaxLength(100);
        builder.Property(d => d.LastName).IsRequired().HasMaxLength(100);
        builder.Property(d => d.PhoneNumber).IsRequired().HasMaxLength(20);
        builder.HasIndex(d => d.PhoneNumber).IsUnique();

        builder.Property(d => d.Email).HasMaxLength(255);
        if (builder.Metadata.FindProperty("Email") != null)
        {
             // Make email unique if provided, EF core handles sparse unique indexes via HasFilter
             builder.HasIndex(d => d.Email).IsUnique().HasFilter("[email] IS NOT NULL");
        }

        builder.Property(d => d.LicenceNumber).IsRequired().HasMaxLength(100);
        builder.Property(d => d.LicenceExpiryDate).IsRequired();
        builder.Property(d => d.NationalityCode).IsRequired().HasMaxLength(10);
        builder.Property(d => d.SponsorName).HasMaxLength(200);

        builder.Property(d => d.Status).IsRequired();
        builder.Property(d => d.IsActive).IsRequired().HasDefaultValue(true);
        builder.Property(d => d.CreatedAt).IsRequired();
        builder.Property(d => d.UpdatedAt).IsRequired();

        // One-to-One
        builder.HasOne(d => d.AuthCredential)
            .WithOne(c => c.Driver)
            .HasForeignKey<DriverAuthCredential>(c => c.DriverId)
            .IsRequired(false) // Driver can exist without auth credentials initially
            .OnDelete(DeleteBehavior.Cascade);

        // One-to-Many
        builder.HasMany(d => d.AttendanceLogs)
            .WithOne(a => a.Driver)
            .HasForeignKey(a => a.DriverId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(d => d.Expenses)
            .WithOne(e => e.Driver)
            .HasForeignKey(e => e.DriverId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(d => d.LocationUpdates)
            .WithOne(l => l.Driver)
            .HasForeignKey(l => l.DriverId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(d => d.Notifications)
            .WithOne(n => n.Driver)
            .HasForeignKey(n => n.DriverId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(d => d.Documents)
            .WithOne(doc => doc.Driver)
            .HasForeignKey(doc => doc.DriverId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
