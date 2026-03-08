using Application.Abstractions.Messaging;
using Domain.Drivers;
using Domain.Drivers.Enums;
using SharedKernel;
namespace Application.Drivers.Assignments.GetAssignments; public sealed record GetAssignmentsQuery(Guid? DriverId = null, AssignmentStatus? Status = null, int Page = 1, int PageSize = 10) : IQuery<PagedList<AssignmentResponse>>;
