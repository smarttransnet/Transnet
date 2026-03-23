using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Assets;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Trailers.DeleteTrailer;

internal sealed class DeleteTrailerCommandHandler(IApplicationDbContext dbContext)
    : ICommandHandler<DeleteTrailerCommand>
{
    public async Task<Result> Handle(DeleteTrailerCommand request, CancellationToken cancellationToken)
    {
        Trailer? trailer = await dbContext.Trailers
            .FirstOrDefaultAsync(t => t.Id == request.Id, cancellationToken);

        if (trailer is null)
        {
            return Result.Failure(Error.NotFound("Trailer.NotFound", "The trailer was not found."));
        }

        dbContext.Trailers.Remove(trailer);

        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
