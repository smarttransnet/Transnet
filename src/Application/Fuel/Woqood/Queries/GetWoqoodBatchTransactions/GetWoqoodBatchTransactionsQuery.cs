using Application.Abstractions.Messaging;

namespace Application.Fuel.Woqood.Queries.GetWoqoodBatchTransactions;

public sealed record GetWoqoodBatchTransactionsQuery(Guid BatchId) : IQuery<IReadOnlyList<WoqoodFuelTransactionResponse>>;
