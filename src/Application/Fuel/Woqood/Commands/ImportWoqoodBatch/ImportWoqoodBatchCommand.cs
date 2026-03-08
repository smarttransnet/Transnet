using Application.Abstractions.Messaging;

namespace Application.Fuel.Woqood.Commands.ImportWoqoodBatch;

public sealed record ImportWoqoodBatchCommand(
    Guid UserId,
    string FileName,
    byte[] FileContents
) : ICommand<Guid>;
