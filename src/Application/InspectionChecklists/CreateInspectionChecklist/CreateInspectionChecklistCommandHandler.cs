using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Inspections;
using SharedKernel;

namespace Application.InspectionChecklists.CreateInspectionChecklist;

internal sealed class CreateInspectionChecklistCommandHandler(IApplicationDbContext dbContext, IDateTimeProvider dateTimeProvider)
    : ICommandHandler<CreateInspectionChecklistCommand, Guid>
{
    public async Task<Result<Guid>> Handle(CreateInspectionChecklistCommand request, CancellationToken cancellationToken)
    {
        var checklist = new InspectionChecklist
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            VehicleCategoryId = request.VehicleCategoryId,
            IsActive = true,
            CreatedAt = dateTimeProvider.UtcNow,
            Items = request.Items.Select(i => new ChecklistItem
            {
                Id = Guid.NewGuid(),
                ItemName = i.ItemName,
                Category = i.Category,
                IsRequired = i.IsRequired,
                SortOrder = i.SortOrder
            }).ToList()
        };

        dbContext.InspectionChecklists.Add(checklist);

        await dbContext.SaveChangesAsync(cancellationToken);

        return checklist.Id;
    }
}
