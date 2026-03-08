using Application.Abstractions.Messaging;
using Domain.Drivers;
using Domain.Drivers.Enums;
namespace Application.Drivers.Auth.CreateCredentials; public sealed record CreateCredentialsCommand(Guid DriverId, string Password, MobilePlatform Platform, string? DeviceToken = null) : ICommand<Guid>;
