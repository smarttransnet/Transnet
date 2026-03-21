using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.InspectionChecklists;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.InspectionChecklists.GetInspectionChecklists;

internal sealed class GetInspectionChecklistsQueryHandler(IApplicationDbContext dbContext)
    : IQueryHandler<GetInspectionChecklistsQuery, IReadOnlyList<InspectionChecklistResponse>>
{
    public async Task<Result<IReadOnlyList<InspectionChecklistResponse>>> Handle(GetInspectionChecklistsQuery request, CancellationToken cancellationToken)
    {
        IReadOnlyList<InspectionChecklistResponse> checklists = await dbContext.InspectionChecklists
            .AsNoTracking()
            .Include(c => c.Items)
            .Select(c => new InspectionChecklistResponse(
                c.Id,
                c.Name,
                c.InspectionType,
                c.ApplicableVehicleTypes,
                c.IsActive,
                c.Items.Select(i => new ChecklistItemResponse(
                    i.Id,
                    i.ItemName,
                    i.Category,
                    i.IsRequired,
                    i.SortOrder)).ToList()))
            .ToListAsync(cancellationToken);

        return Result.Success(checklists);
    }
}
