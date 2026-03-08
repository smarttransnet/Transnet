using Application.Abstractions.Messaging;
using Domain.Drivers;
namespace Application.Drivers.Auth.Logout; public sealed record LogoutCommand(Guid DriverId) : ICommand;
