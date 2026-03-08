using Application.Abstractions.Messaging;

namespace Application.Drivers.Assignments.RejectAssignment;

public sealed record RejectAssignmentCommand(
    Guid AssignmentId,
    string? RejectionReason = null) : ICommand;
