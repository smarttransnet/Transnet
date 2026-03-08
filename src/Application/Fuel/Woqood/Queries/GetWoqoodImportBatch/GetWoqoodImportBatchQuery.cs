using Application.Abstractions.Messaging;

namespace Application.Fuel.Woqood.Queries.GetWoqoodImportBatch;

public sealed record GetWoqoodImportBatchQuery(Guid BatchId) : IQuery<WoqoodImportBatchResponse>;
