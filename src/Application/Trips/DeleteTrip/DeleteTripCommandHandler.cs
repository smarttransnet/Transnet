using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Trips;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Trips.DeleteTrip;

internal sealed class DeleteTripCommandHandler(IApplicationDbContext dbContext)
    : ICommandHandler<DeleteTripCommand>
{
    public async Task<Result> Handle(DeleteTripCommand request, CancellationToken cancellationToken)
    {
        Trip? trip = await dbContext.Trips
            .FirstOrDefaultAsync(t => t.Id == request.Id, cancellationToken);

        if (trip is null)
        {
            return Result.Failure(Error.NotFound("Trip.NotFound", "The trip was not found."));
        }

        bool hasInvoices = await dbContext.InvoiceTripLinks
            .AnyAsync(link => link.TripId == trip.Id, cancellationToken);

        if (hasInvoices)
        {
            trip.Status = Domain.Trips.Enums.TripStatus.Deleted;
            trip.UpdatedAt = DateTime.UtcNow;
        }
        else
        {
            dbContext.Trips.Remove(trip);
        }

        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
