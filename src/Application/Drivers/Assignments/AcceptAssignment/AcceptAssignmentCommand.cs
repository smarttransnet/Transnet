using Application.Abstractions.Messaging;
using Domain.Drivers;
namespace Application.Drivers.Assignments.AcceptAssignment; public sealed record AcceptAssignmentCommand(Guid AssignmentId) : ICommand;
