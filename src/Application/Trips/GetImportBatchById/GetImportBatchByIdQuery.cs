using Application.Abstractions.Messaging;
using Application.Trips.Common;

namespace Application.Trips.GetImportBatchById;

public sealed record GetImportBatchByIdQuery(Guid Id) : IQuery<ImportBatchResponse>;
