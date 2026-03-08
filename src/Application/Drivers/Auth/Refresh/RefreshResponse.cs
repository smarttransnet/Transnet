using Application.Abstractions.Messaging;
using Domain.Drivers;
namespace Application.Drivers.Auth.Refresh; public sealed record RefreshResponse(string AccessToken, string RefreshToken);
