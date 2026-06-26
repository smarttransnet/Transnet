using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.InspectionChecklists.GetInspectionChecklists;

internal sealed class GetInspectionChecklistsQueryHandler(IApplicationDbContext dbContext)
    : IQueryHandler<GetInspectionChecklistsQuery, List<InspectionChecklistResponse>>
{
    public async Task<Result<List<InspectionChecklistResponse>>> Handle(GetInspectionChecklistsQuery request, CancellationToken cancellationToken)
    {
        List<InspectionChecklistResponse> checklists = await dbContext.InspectionChecklists
            .AsNoTracking()
            .Include(c => c.Items)
            .Select(c => new InspectionChecklistResponse(
                c.Id,
                c.Name,
                c.VehicleCategoryId,
                c.IsActive,
                c.Items.Select(i => new ChecklistItemResponse(
                    i.Id,
                    i.ItemName,
                    i.Category,
                    i.IsRequired,
                    i.SortOrder)).ToList()))
            .ToListAsync(cancellationToken);

        return checklists;
    }
}
