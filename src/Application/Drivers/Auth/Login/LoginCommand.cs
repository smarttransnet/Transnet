using Application.Abstractions.Messaging;

namespace Application.Drivers.Auth.Login;

public sealed record LoginCommand(
    string Username,
    string Password,
    string? Platform = null,
    string? DeviceToken = null) : ICommand<LoginResponse>;
