using Application.Abstractions.Messaging;
using SharedKernel;

namespace Application.Trailers.UpdateTrailer;

public sealed record UpdateTrailerCommand(
    Guid Id,
    string TrailerNumber,
    string TrailerType,
    decimal Capacity,
    string CapacityUnit,
    bool IsActive) : ICommand;
