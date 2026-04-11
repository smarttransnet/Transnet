using Application.Abstractions.Messaging;

namespace Application.Invoices.Commands.CreateInvoiceFromTrips;

public sealed record CreateInvoiceFromTripsCommand(
    Guid ClientId,
    List<Guid> TripIds,
    decimal UnitPricePerTrip,
    DateTime DueDate,
    string? Notes,
    Guid IssuedByUserId) : ICommand<Guid>;
