using System.Globalization;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.WorkOrders;
using Domain.WorkOrders.Enums;
using SharedKernel;

namespace Application.WorkOrders.CreateWorkOrder;

internal sealed class CreateWorkOrderCommandHandler(
    IApplicationDbContext dbContext,
    IDateTimeProvider dateTimeProvider)
    : ICommandHandler<CreateWorkOrderCommand, Guid>
{
    public async Task<Result<Guid>> Handle(CreateWorkOrderCommand request, CancellationToken cancellationToken)
    {
        DateTime now = dateTimeProvider.UtcNow;
        string workOrderNumber = $"WO-{now:yyyyMMdd}-{Guid.NewGuid().ToString()[..6].ToUpper(CultureInfo.InvariantCulture)}";

        var workOrder = new WorkOrder
        {
            Id = Guid.NewGuid(),
            WorkOrderNumber = workOrderNumber,
            Title = request.Title,
            Description = request.Description,
            VehicleId = request.VehicleId,
            VehicleInspectionId = request.VehicleInspectionId,
            Priority = request.Priority,
            Status = WorkOrderStatus.Open,
            AssignedTechnicianId = request.AssignedTechnicianId,
            EstimatedCostQAR = request.EstimatedCostQAR,
            ScheduledDate = request.ScheduledDate,
            JobType = request.JobType,
            DriverName = request.DriverName,
            PreparedBy = request.PreparedBy,
            CheckedByDriver = request.CheckedByDriver,
            CheckedByMechanic = request.CheckedByMechanic,
            AuthorizedBy = request.AuthorizedBy,
            StartedAt = request.StartedAt,
            CompletedAt = request.CompletedAt,
            WorkOrderItems = request.Items.Select(i => new WorkOrderItem
            {
                Id = Guid.NewGuid(),
                ItemType = i.ItemType,
                Description = i.Description,
                Quantity = i.Quantity,
                UnitCostQAR = i.UnitCostQAR,
                TotalCostQAR = i.Quantity * i.UnitCostQAR,
                StartTime = i.StartTime,
                EndTime = i.EndTime,
                MechanicName = i.MechanicName,
                Remarks = i.Remarks
            }).ToList(),
            StatusHistory = new List<WorkOrderStatusHistory>
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    PreviousStatus = WorkOrderStatus.Open,
                    NewStatus = WorkOrderStatus.Open,
                    Notes = "Work Order Created",
                    ChangedByUserId = request.AssignedTechnicianId ?? Guid.Empty, // Default to empty if not provided or use a system user
                    ChangedAt = now
                }
            },
            CreatedAt = now,
            UpdatedAt = now
        };

        dbContext.WorkOrders.Add(workOrder);

        await dbContext.SaveChangesAsync(cancellationToken);

        return workOrder.Id;
    }
}
