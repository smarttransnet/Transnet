using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Assets;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Trailers.DeleteTrailer;

internal sealed class DeleteTrailerCommandHandler(IApplicationDbContext context)
    : ICommandHandler<DeleteTrailerCommand>
{
    public async Task<Result> Handle(DeleteTrailerCommand command, CancellationToken cancellationToken)
    {
        Trailer? trailer = await context.Trailers
            .FirstOrDefaultAsync(t => t.Id == command.Id, cancellationToken);

        if (trailer is null)
        {
            return Result.Failure(Error.NotFound("Trailers.NotFound", $"The trailer with the Id = '{command.Id}' was not found"));
        }

        context.Trailers.Remove(trailer);

        await context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
