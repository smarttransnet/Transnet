using Domain.WorkOrders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.WorkOrders;

internal sealed class WorkOrderConfiguration : IEntityTypeConfiguration<WorkOrder>
{
    public void Configure(EntityTypeBuilder<WorkOrder> builder)
    {
        builder.HasKey(o => o.Id);
        
        builder.Property(o => o.WorkOrderNumber).IsRequired().HasMaxLength(50);
        builder.Property(o => o.Title).IsRequired().HasMaxLength(200);
        builder.Property(o => o.Description).HasMaxLength(2000);

        builder.Property(o => o.EstimatedCostQAR).HasColumnType("decimal(18,2)");
        builder.Property(o => o.ActualCostQAR).HasColumnType("decimal(18,2)");

        builder.Property(o => o.ScheduledDate).HasConversion(d => d.HasValue ? DateTime.SpecifyKind(d.Value, DateTimeKind.Utc) : d, v => v);
        builder.Property(o => o.StartedAt).HasConversion(d => d.HasValue ? DateTime.SpecifyKind(d.Value, DateTimeKind.Utc) : d, v => v);
        builder.Property(o => o.CompletedAt).HasConversion(d => d.HasValue ? DateTime.SpecifyKind(d.Value, DateTimeKind.Utc) : d, v => v);
        builder.Property(o => o.CreatedAt).HasConversion(d => DateTime.SpecifyKind(d, DateTimeKind.Utc), v => v);
        builder.Property(o => o.UpdatedAt).HasConversion(d => DateTime.SpecifyKind(d, DateTimeKind.Utc), v => v);

        builder.HasOne(o => o.Vehicle)
            .WithMany()
            .HasForeignKey(o => o.VehicleId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(o => o.VehicleInspection)
            .WithMany()
            .HasForeignKey(o => o.VehicleInspectionId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
