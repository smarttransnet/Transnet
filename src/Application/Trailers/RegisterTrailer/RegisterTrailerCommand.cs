using Application.Abstractions.Messaging;
using Domain.Assets.Enums;

namespace Application.Trailers.RegisterTrailer;

public sealed record RegisterTrailerCommand(
    string TrailerNumber,
    string TrailerType,
    decimal Capacity,
    string CapacityUnit,
    TrailerStatus Status) : ICommand<Guid>;
