using Application.Abstractions.Messaging;

namespace Application.Reports.GetMaintenanceCosts;

public sealed record MaintenanceCostResponse(
    Guid VehicleId,
    string RegistrationNumber,
    int TotalWorkOrders,
    decimal TotalCostQAR);

public sealed record GetMaintenanceCostsQuery(DateTime StartDate, DateTime EndDate) : IQuery<List<MaintenanceCostResponse>>;
