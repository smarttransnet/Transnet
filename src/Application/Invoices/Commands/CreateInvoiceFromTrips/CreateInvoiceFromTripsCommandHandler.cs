using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Billing;
using Domain.Billing.Enums;
using Domain.Trips;
using Domain.Trips.Enums;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Invoices.Commands.CreateInvoiceFromTrips;

internal sealed class CreateInvoiceFromTripsCommandHandler(
    IApplicationDbContext dbContext)
    : ICommandHandler<CreateInvoiceFromTripsCommand, Guid>
{
    public async Task<Result<Guid>> Handle(CreateInvoiceFromTripsCommand request, CancellationToken cancellationToken)
    {
        // 1. Validate Client
        bool clientExists = await dbContext.Clients.AnyAsync(c => c.Id == request.ClientId, cancellationToken);
        if (!clientExists)
        {
            return Result.Failure<Guid>(Error.NotFound("Client.NotFound", "The specified client was not found."));
        }

        // 2. Fetch Trips
        var trips = await dbContext.Trips
            .Where(t => request.TripIds.Contains(t.Id))
            .ToListAsync(cancellationToken);

        if (trips.Count != request.TripIds.Count)
        {
            return Result.Failure<Guid>(Error.Failure("Trips.SomeNotFound", "Some of the specified trips were not found."));
        }

        // 3. Validate Trips eligibility
        foreach (var trip in trips)
        {
            if (trip.ClientId != request.ClientId)
            {
                return Result.Failure<Guid>(Error.Failure("Trip.InvalidClient", $"Trip {trip.TripNumber} does not belong to the specified client."));
            }

            // Check if already linked to an invoice
            bool alreadyLinked = await dbContext.InvoiceTripLinks.AnyAsync(l => l.TripId == trip.Id, cancellationToken);
            if (alreadyLinked)
            {
                return Result.Failure<Guid>(Error.Conflict("Trip.AlreadyLinked", $"Trip {trip.TripNumber} is already linked to an invoice."));
            }
        }

        // 4. Generate Invoice Number
        string datePrefix = DateTime.UtcNow.ToString("yyyyMM", System.Globalization.CultureInfo.InvariantCulture);
        int count = await dbContext.Invoices.CountAsync(i => i.InvoiceNumber.StartsWith($"INV-{datePrefix}"), cancellationToken);
        string invoiceNumber = $"INV-{datePrefix}-{(count + 1).ToString(System.Globalization.CultureInfo.InvariantCulture).PadLeft(4, '0')}";

        // 5. Create Invoice
        var invoice = new Invoice
        {
            Id = Guid.NewGuid(),
            InvoiceNumber = invoiceNumber,
            ClientId = request.ClientId,
            IssuedByUserId = request.IssuedByUserId,
            IssuedAt = DateTime.UtcNow,
            DueDate = DateOnly.FromDateTime(request.DueDate),
            Status = InvoiceStatus.Draft,
            Notes = request.Notes,
            SubTotalQAR = 0,
            TaxAmountQAR = 0,
            TotalQAR = 0,
            OutstandingAmountQAR = 0
        };

        decimal subTotal = 0m;

        // 6. Generate Line Items and Links
        foreach (var trip in trips)
        {
            decimal lineTotal = request.UnitPricePerTrip;
            subTotal += lineTotal;

            // Add Line Item
            invoice.LineItems.Add(new InvoiceLineItem
            {
                Id = Guid.NewGuid(),
                InvoiceId = invoice.Id,
                Description = $"Transportation Service: {trip.TripNumber} ({trip.Origin} to {trip.Destination})",
                ServiceType = ServiceType.FreightTransport,
                Quantity = 1,
                UnitPriceQAR = request.UnitPricePerTrip,
                DiscountPercent = 0,
                TaxPercent = 0,
                LineTotalQAR = lineTotal,
                TripId = trip.Id,
                SortOrder = invoice.LineItems.Count + 1
            });

            // Add Trip link (Relational)
            invoice.TripLinks.Add(new InvoiceTripLink
            {
                Id = Guid.NewGuid(),
                InvoiceId = invoice.Id,
                TripId = trip.Id,
                LinkedAt = DateTime.UtcNow,
                LinkedByUserId = request.IssuedByUserId,
                TripCompletionVerified = trip.Status == TripStatus.Invoiced || trip.Status == TripStatus.Completed
            });
        }

        invoice.SubTotalQAR = subTotal;
        invoice.TotalQAR = subTotal; // Tax is 0 as per user request
        invoice.OutstandingAmountQAR = subTotal;

        dbContext.Invoices.Add(invoice);
        await dbContext.SaveChangesAsync(cancellationToken);

        return invoice.Id;
    }
}
