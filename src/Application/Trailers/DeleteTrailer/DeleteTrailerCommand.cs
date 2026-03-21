using Application.Abstractions.Messaging;

namespace Application.Trailers.DeleteTrailer;

public sealed record DeleteTrailerCommand(Guid Id) : ICommand;
