using Application.Abstractions.Messaging;
using Application.Trips.Common;

namespace Application.Trips.GetImportBatches;

public sealed record GetImportBatchesQuery() : IQuery<List<ImportBatchResponse>>;
