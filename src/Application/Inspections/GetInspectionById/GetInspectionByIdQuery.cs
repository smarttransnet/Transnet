using Application.Abstractions.Messaging;

namespace Application.Inspections.GetInspectionById;

public sealed record GetInspectionByIdQuery(Guid Id) : IQuery<InspectionDetailedResponse>;
