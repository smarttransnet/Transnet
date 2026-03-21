using Application.Abstractions.Messaging;
using Application.InspectionChecklists;
using Domain.Inspections.Enums;

namespace Application.InspectionChecklists.GetInspectionChecklists;

public sealed record GetInspectionChecklistsQuery : IQuery<IReadOnlyList<InspectionChecklistResponse>>;
