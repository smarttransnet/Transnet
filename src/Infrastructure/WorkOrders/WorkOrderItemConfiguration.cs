using Domain.WorkOrders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.WorkOrders;

internal sealed class WorkOrderItemConfiguration : IEntityTypeConfiguration<WorkOrderItem>
{
    public void Configure(EntityTypeBuilder<WorkOrderItem> builder)
    {
        builder.HasKey(i => i.Id);
        
        builder.Property(i => i.Description).IsRequired().HasMaxLength(500);

        builder.Property(i => i.Quantity).HasColumnType("decimal(18,4)");
        builder.Property(i => i.UnitCostQAR).HasColumnType("decimal(18,2)");
        builder.Property(i => i.TotalCostQAR).HasColumnType("decimal(18,2)");

        builder.HasOne(i => i.WorkOrder)
            .WithMany(o => o.WorkOrderItems)
            .HasForeignKey(i => i.WorkOrderId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
