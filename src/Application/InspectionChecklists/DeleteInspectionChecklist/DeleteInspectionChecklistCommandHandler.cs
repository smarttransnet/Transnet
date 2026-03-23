using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Inspections;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.InspectionChecklists.DeleteInspectionChecklist;

internal sealed class DeleteInspectionChecklistCommandHandler(IApplicationDbContext dbContext)
    : ICommandHandler<DeleteInspectionChecklistCommand>
{
    public async Task<Result> Handle(DeleteInspectionChecklistCommand request, CancellationToken cancellationToken)
    {
        InspectionChecklist? checklist = await dbContext.InspectionChecklists
            .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

        if (checklist is null)
        {
            return Result.Failure(Error.NotFound("InspectionChecklist.NotFound", "The inspection checklist was not found."));
        }

        dbContext.InspectionChecklists.Remove(checklist);

        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
