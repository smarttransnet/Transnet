using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Drivers;
using Domain.Drivers.Enums;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Drivers.Assignments.AcceptAssignment;

internal sealed class AcceptAssignmentCommandHandler(
    IApplicationDbContext dbContext,
    IDateTimeProvider dateTimeProvider) : ICommandHandler<AcceptAssignmentCommand>
{
    public async Task<Result> Handle(AcceptAssignmentCommand request, CancellationToken cancellationToken)
    {
        DriverTripAssignment? assignment = await dbContext.DriverTripAssignments
            .FirstOrDefaultAsync(a => a.Id == request.AssignmentId, cancellationToken);

        if (assignment is null)
        {
            return Result.Failure(Error.NotFound("Assignments.NotFound", "The trip assignment was not found."));
        }

        if (assignment.AssignmentStatus != AssignmentStatus.Pending)
        {
            return Result.Failure(Error.Conflict("Assignments.NotPending", "Only pending assignments can be accepted."));
        }

        assignment.AssignmentStatus = AssignmentStatus.Accepted;
        assignment.AcceptedAt = dateTimeProvider.UtcNow;
        // Optionally, one might also update the actual Trip status here from 'Scheduled' to 'DriverConfirmed' if cross-module communication is established.
        // For simplicity, we just update the assignment. Real implementations often use Domain Events for cross-aggregate updates.

        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
