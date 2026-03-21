using Application.Abstractions.Messaging;

namespace Application.InspectionChecklists.GetInspectionChecklistById;

public sealed record GetInspectionChecklistByIdQuery(Guid Id) : IQuery<InspectionChecklistResponse>;
