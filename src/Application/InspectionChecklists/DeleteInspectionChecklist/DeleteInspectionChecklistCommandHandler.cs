using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Inspections;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.InspectionChecklists.DeleteInspectionChecklist;

internal sealed class DeleteInspectionChecklistCommandHandler(IApplicationDbContext context)
    : ICommandHandler<DeleteInspectionChecklistCommand>
{
    public async Task<Result> Handle(DeleteInspectionChecklistCommand command, CancellationToken cancellationToken)
    {
        InspectionChecklist? checklist = await context.InspectionChecklists
            .Include(c => c.Items)
            .FirstOrDefaultAsync(c => c.Id == command.Id, cancellationToken);

        if (checklist is null)
        {
            return Result.Failure(Error.NotFound("InspectionChecklists.NotFound", $"The inspection checklist with the Id = '{command.Id}' was not found"));
        }

        context.InspectionChecklists.Remove(checklist);

        await context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
