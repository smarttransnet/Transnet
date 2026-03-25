using Application.Abstractions.Messaging;

namespace Application.Inspections.GetInspections;

public sealed record GetInspectionsQuery(
    Guid? VehicleId,
    int PageNumber = 1,
    int PageSize = 20) : IQuery<List<InspectionResponse>>;
