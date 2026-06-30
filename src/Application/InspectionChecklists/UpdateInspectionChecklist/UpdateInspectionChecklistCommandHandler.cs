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
        InspectionChecklist? checklist = await dbContext.InspectionChecklists
            .Include(c => c.Items)
            .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

        if (checklist is null)
        {
            return Result.Failure(Error.NotFound("InspectionChecklist.NotFound", "The inspection checklist was not found."));
        }

        checklist.Name = request.Name;
        checklist.VehicleCategoryId = request.VehicleCategoryId;
        checklist.IsActive = request.IsActive;

        // Update items
        // Remove items not in the request
        var itemIdsToKeep = request.Items.Where(i => i.Id.HasValue).Select(i => i.Id!.Value).ToList();
        var itemsToRemove = checklist.Items.Where(i => !itemIdsToKeep.Contains(i.Id)).ToList();
        foreach (var item in itemsToRemove)
        {
            checklist.Items.Remove(item);
        }

        foreach (var itemCommand in request.Items)
        {
             if (itemCommand.Id.HasValue)
             {
                 var existingItem = checklist.Items.FirstOrDefault(i => i.Id == itemCommand.Id!.Value);
                 if (existingItem != null)
                 {
                     existingItem.ItemName = itemCommand.ItemName;
                     existingItem.Category = itemCommand.Category;
                     existingItem.IsRequired = itemCommand.IsRequired;
                     existingItem.SortOrder = itemCommand.SortOrder;
                 }
                 else
                 {
                     checklist.Items.Add(new ChecklistItem
                     {
                         Id = Guid.NewGuid(),
                         InspectionChecklistId = checklist.Id,
                         ItemName = itemCommand.ItemName,
                         Category = itemCommand.Category,
                         IsRequired = itemCommand.IsRequired,
                         SortOrder = itemCommand.SortOrder
                     });
                 }
             }
             else
             {
                 checklist.Items.Add(new ChecklistItem
                 {
                     Id = Guid.NewGuid(),
                     InspectionChecklistId = checklist.Id,
                     ItemName = itemCommand.ItemName,
                     Category = itemCommand.Category,
                     IsRequired = itemCommand.IsRequired,
                     SortOrder = itemCommand.SortOrder
                 });
             }
        }

        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
