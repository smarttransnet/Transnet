using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Drivers;
using Domain.Drivers.Enums;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Drivers.Assignments.RejectAssignment;

internal sealed class RejectAssignmentCommandHandler(
    IApplicationDbContext dbContext,
    IDateTimeProvider dateTimeProvider) : ICommandHandler<RejectAssignmentCommand>
{
    public async Task<Result> Handle(RejectAssignmentCommand request, CancellationToken cancellationToken)
    {
        DriverTripAssignment? assignment = await dbContext.DriverTripAssignments
            .FirstOrDefaultAsync(a => a.Id == request.AssignmentId, cancellationToken);

        if (assignment is null)
        {
            return Result.Failure(Error.NotFound("Assignments.NotFound", "The trip assignment was not found."));
        }

        if (assignment.AssignmentStatus != AssignmentStatus.Pending)
        {
            return Result.Failure(Error.Conflict("Assignments.NotPending", "Only pending assignments can be rejected."));
        }

        assignment.AssignmentStatus = AssignmentStatus.Rejected;
        assignment.RejectedAt = dateTimeProvider.UtcNow;
        assignment.RejectionReason = request.RejectionReason;

        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
