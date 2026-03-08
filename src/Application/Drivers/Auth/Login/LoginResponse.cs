using Application.Abstractions.Messaging;
using Domain.Drivers;
namespace Application.Drivers.Auth.Login; public sealed record LoginResponse(string AccessToken, string RefreshToken, Guid DriverId, string FirstName, string LastName);
