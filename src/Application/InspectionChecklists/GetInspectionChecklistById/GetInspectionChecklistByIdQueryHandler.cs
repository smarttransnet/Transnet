using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.InspectionChecklists.GetInspectionChecklistById;

internal sealed class GetInspectionChecklistByIdQueryHandler(IApplicationDbContext dbContext)
    : IQueryHandler<GetInspectionChecklistByIdQuery, InspectionChecklistResponse>
{
    public async Task<Result<InspectionChecklistResponse>> Handle(GetInspectionChecklistByIdQuery request, CancellationToken cancellationToken)
    {
        var checklist = await dbContext.InspectionChecklists
            .AsNoTracking()
            .Include(c => c.Items)
            .Where(c => c.Id == request.Id)
            .Select(c => new InspectionChecklistResponse(
                c.Id,
                c.Name,
                c.InspectionType,
                c.ApplicableVehicleTypes,
                c.IsActive,
                c.Items.OrderBy(i => i.SortOrder).Select(i => new ChecklistItemResponse(
                    i.Id,
                    i.ItemName,
                    i.Category,
                    i.IsRequired,
                    i.SortOrder)).ToList()))
            .FirstOrDefaultAsync(cancellationToken);

        if (checklist is null)
        {
            return Result.Failure<InspectionChecklistResponse>(Error.NotFound("InspectionChecklist.NotFound", $"The inspection checklist with ID {request.Id} was not found."));
        }

        return checklist;
    }
}
