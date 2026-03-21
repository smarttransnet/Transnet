using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Drivers;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Drivers.Assignments.GetAssignments;

internal sealed class GetAssignmentsQueryHandler(IApplicationDbContext context)
    : IQueryHandler<GetAssignmentsQuery, PagedList<AssignmentResponse>>
{
    public async Task<Result<PagedList<AssignmentResponse>>> Handle(GetAssignmentsQuery query, CancellationToken cancellationToken)
    {
        IQueryable<DriverTripAssignment> assignmentsQuery = context.DriverTripAssignments;

        if (query.DriverId.HasValue)
        {
            assignmentsQuery = assignmentsQuery.Where(a => a.DriverId == query.DriverId.Value);
        }

        if (query.Status.HasValue)
        {
            assignmentsQuery = assignmentsQuery.Where(a => a.AssignmentStatus == query.Status.Value);
        }

        int totalCount = await assignmentsQuery.CountAsync(cancellationToken);

        List<AssignmentResponse> assignments = await assignmentsQuery
            .OrderByDescending(a => a.AssignedAt)
            .Skip((query.Page - 1) * query.PageSize)
            .Take(query.PageSize)
            .Select(a => new AssignmentResponse(
                a.Id,
                a.DriverId,
                a.TripId,
                a.AssignedAt,
                a.AssignedByUserId,
                a.AcceptedAt,
                a.RejectedAt,
                a.RejectionReason,
                a.AssignmentStatus,
                a.DisplayedInAppAt))
            .ToListAsync(cancellationToken);

        return new PagedList<AssignmentResponse>(assignments, totalCount, query.Page, query.PageSize);
    }
}
