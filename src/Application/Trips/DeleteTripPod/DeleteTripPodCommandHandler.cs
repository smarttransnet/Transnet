using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Trips;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Trips.DeleteTripPod;

internal sealed class DeleteTripPodCommandHandler : ICommandHandler<DeleteTripPodCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteTripPodCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result> Handle(DeleteTripPodCommand request, CancellationToken cancellationToken)
    {
        TripPodUpload? upload = await _context.TripPodUploads
            .FirstOrDefaultAsync(p => p.Id == request.Id && p.TripId == request.TripId, cancellationToken);

        if (upload is null)
        {
            return Result.Failure(Error.NotFound("TripPodUpload.NotFound", $"Trip POD upload with ID {request.Id} for trip {request.TripId} was not found."));
        }

        _context.TripPodUploads.Remove(upload);
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
