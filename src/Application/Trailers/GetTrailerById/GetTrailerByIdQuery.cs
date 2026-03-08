using Application.Abstractions.Messaging;

namespace Application.Trailers.GetTrailerById;

public sealed record GetTrailerByIdQuery(Guid TrailerId) : IQuery<GetTrailers.TrailerResponse>;
