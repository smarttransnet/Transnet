using Application.Abstractions.Messaging;

namespace Application.Fuel.Summaries.Queries.GetVehicleFuelSummaries;

public sealed record GetVehicleFuelSummariesQuery(
    Guid? VehicleId = null,
    int? PeriodMonth = null,
    int? PeriodYear = null
) : IQuery<IReadOnlyList<VehicleFuelSummaryResponse>>;
