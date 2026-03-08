using Application.Abstractions.Messaging;

namespace Application.Drivers.Auth.Refresh;

public sealed record RefreshCommand(
    Guid DriverId,
    string RefreshToken) : ICommand<RefreshResponse>;
