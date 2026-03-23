using Application.Abstractions.Messaging;

namespace Application.InspectionChecklists.DeleteInspectionChecklist;

public sealed record DeleteInspectionChecklistCommand(Guid Id) : ICommand;
