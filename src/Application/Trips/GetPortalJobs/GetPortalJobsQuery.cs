using Application.Abstractions.Messaging;
using Application.Trips.Common;

namespace Application.Trips.GetPortalJobs;

public sealed record GetPortalJobsQuery(Guid ClientId) : IQuery<List<TripResponse>>;
