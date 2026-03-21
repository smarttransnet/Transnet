using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Inspections;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.InspectionChecklists.UpdateInspectionChecklist;

internal sealed class UpdateInspectionChecklistCommandHandler(IApplicationDbContext dbContext)
    : ICommandHandler<UpdateInspectionChecklistCommand>
{
    public async Task<Result> Handle(UpdateInspectionChecklistCommand request, CancellationToken cancellationToken)
    {
        var checklist = await dbContext.InspectionChecklists
            .Include(c => c.Items)
            .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

        if (checklist is null)
        {
            return Result.Failure(Error.NotFound("InspectionChecklist.NotFound", $"The inspection checklist with ID {request.Id} was not found."));
        }

        checklist.Name = request.Name;
        checklist.InspectionType = request.InspectionType;
        checklist.ApplicableVehicleTypes = request.ApplicableVehicleTypes;
        checklist.IsActive = request.IsActive;

        // Handle items
        var existingItems = checklist.Items.ToList();
        var incomingItemIds = request.Items.Where(i => i.Id.HasValue).Select(i => i.Id!.Value).ToList();

        // Remove items not in request
        foreach (var existingItem in existingItems)
        {
            if (!incomingItemIds.Contains(existingItem.Id))
            {
                dbContext.ChecklistItems.Remove(existingItem);
            }
        }

        // Update or add items
        foreach (var itemCommand in request.Items)
        {
            if (itemCommand.Id.HasValue)
            {
                var existingItem = existingItems.FirstOrDefault(i => i.Id == itemCommand.Id.Value);
                if (existingItem != null)
                {
                    existingItem.ItemName = itemCommand.ItemName;
                    existingItem.Category = itemCommand.Category;
                    existingItem.IsRequired = itemCommand.IsRequired;
                    existingItem.SortOrder = itemCommand.SortOrder;
                }
            }
            else
            {
                var newItem = new ChecklistItem
                {
                    Id = Guid.NewGuid(),
                    InspectionChecklistId = checklist.Id,
                    ItemName = itemCommand.ItemName,
                    Category = itemCommand.Category,
                    IsRequired = itemCommand.IsRequired,
                    SortOrder = itemCommand.SortOrder
                };
                dbContext.ChecklistItems.Add(newItem);
            }
        }

        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
