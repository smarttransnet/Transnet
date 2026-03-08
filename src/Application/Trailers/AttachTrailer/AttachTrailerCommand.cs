using Application.Abstractions.Messaging;

namespace Application.Trailers.AttachTrailer;

public sealed record AttachTrailerCommand(Guid TrailerId, Guid VehicleId) : ICommand;
