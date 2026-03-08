using Application.Abstractions.Messaging;

namespace Application.Trailers.GetTrailers;

public sealed record GetTrailersQuery : IQuery<List<TrailerResponse>>;
