using Application.Abstractions.Messaging;

namespace Application.Trailers.UpdateTrailer;

public sealed record UpdateTrailerCommand(
    Guid Id,
    string TrailerNumber,
    string TrailerType,
    decimal Capacity,
    string CapacityUnit,
    bool IsActive) : ICommand;
