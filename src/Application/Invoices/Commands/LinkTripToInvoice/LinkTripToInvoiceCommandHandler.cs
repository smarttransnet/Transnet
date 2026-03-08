using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Billing;
using Domain.Trips;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Invoices.Commands.LinkTripToInvoice;

internal sealed class LinkTripToInvoiceCommandHandler(
    IApplicationDbContext dbContext)
    : ICommandHandler<LinkTripToInvoiceCommand>
{
    public async Task<Result> Handle(LinkTripToInvoiceCommand request, CancellationToken cancellationToken)
    {
        bool invoiceExists = await dbContext.Invoices
            .AnyAsync(i => i.Id == request.InvoiceId, cancellationToken);
            
        if (!invoiceExists)
        {
            return Result.Failure(Error.NotFound("Invoice.NotFound", "The specified invoice was not found."));
        }

        Trip? trip = await dbContext.Trips
            .FirstOrDefaultAsync(t => t.Id == request.TripId, cancellationToken);

        if (trip is null)
        {
            return Result.Failure(Error.NotFound("Trip.NotFound", "The specified trip was not found."));
        }

        bool linkExists = await dbContext.InvoiceTripLinks
            .AnyAsync(l => l.InvoiceId == request.InvoiceId && l.TripId == request.TripId, cancellationToken);

        if (linkExists)
        {
            return Result.Failure(Error.Conflict("InvoiceTripLink.Exists", "This trip is already linked to the invoice."));
        }

        bool isTripVerified = trip.Status == Domain.Trips.Enums.TripStatus.Completed;

        var link = new InvoiceTripLink
        {
            Id = Guid.NewGuid(),
            InvoiceId = request.InvoiceId,
            TripId = request.TripId,
            LinkedAt = DateTime.UtcNow,
            LinkedByUserId = request.LinkedByUserId,
            TripCompletionVerified = isTripVerified
        };

        dbContext.InvoiceTripLinks.Add(link);
        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
