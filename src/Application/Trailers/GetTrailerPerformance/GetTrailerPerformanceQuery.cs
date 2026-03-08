using Application.Abstractions.Messaging;

namespace Application.Trailers.GetTrailerPerformance;

public sealed record TrailerPerformanceResponse(
    Guid TrailerId,
    string TrailerNumber,
    decimal TotalRevenueQAR,
    decimal TotalExpensesQAR,
    decimal NetProfitQAR);

public sealed record GetTrailerPerformanceQuery(Guid TrailerId) : IQuery<TrailerPerformanceResponse>;
