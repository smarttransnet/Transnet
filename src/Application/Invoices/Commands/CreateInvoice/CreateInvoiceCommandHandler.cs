using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Billing;
using Domain.Billing.Enums;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Invoices.Commands.CreateInvoice;

internal sealed class CreateInvoiceCommandHandler(
    IApplicationDbContext dbContext)
    : ICommandHandler<CreateInvoiceCommand, Guid>
{
    public async Task<Result<Guid>> Handle(CreateInvoiceCommand request, CancellationToken cancellationToken)
    {
        bool clientExists = await dbContext.Clients.AnyAsync(c => c.Id == request.ClientId, cancellationToken);
        if (!clientExists)
        {
            return Result.Failure<Guid>(Error.NotFound("Client.NotFound", "The specified client was not found."));
        }

        string datePrefix = DateTime.UtcNow.ToString("yyyyMM", System.Globalization.CultureInfo.InvariantCulture);
        int count = await dbContext.Invoices.CountAsync(i => i.InvoiceNumber.StartsWith($"INV-{datePrefix}"), cancellationToken);
        string invoiceNumber = $"INV-{datePrefix}-{(count + 1).ToString(System.Globalization.CultureInfo.InvariantCulture).PadLeft(4, '0')}";

        var invoice = new Invoice
        {
            Id = Guid.NewGuid(),
            InvoiceNumber = invoiceNumber,
            ClientId = request.ClientId,
            IssuedByUserId = request.IssuedByUserId,
            IssuedAt = DateTime.UtcNow,
            DueDate = request.DueDate,
            Status = InvoiceStatus.Draft,
            Notes = request.Notes
        };

        decimal subTotal = 0m;
        decimal taxAmount = 0m;

        foreach (InvoiceLineItemRequest item in request.LineItems)
        {
            decimal lineGross = item.Quantity * item.UnitPriceQAR;
            decimal discountAmount = lineGross * (item.DiscountPercent / 100m);
            decimal lineNet = lineGross - discountAmount;
            decimal lineTax = lineNet * (item.TaxPercent / 100m);
            decimal lineTotal = lineNet + lineTax;

            subTotal += lineNet;
            taxAmount += lineTax;

            invoice.LineItems.Add(new InvoiceLineItem
            {
                Id = Guid.NewGuid(),
                InvoiceId = invoice.Id,
                Description = item.Description,
                ServiceType = item.ServiceType,
                Quantity = item.Quantity,
                UnitPriceQAR = item.UnitPriceQAR,
                DiscountPercent = item.DiscountPercent,
                TaxPercent = item.TaxPercent,
                LineTotalQAR = lineTotal,
                SortOrder = item.SortOrder,
                TripId = item.TripId
            });
        }

        invoice.SubTotalQAR = subTotal;
        invoice.TaxAmountQAR = taxAmount;
        invoice.TotalQAR = subTotal + taxAmount;
        invoice.OutstandingAmountQAR = invoice.TotalQAR;

        dbContext.Invoices.Add(invoice);
        await dbContext.SaveChangesAsync(cancellationToken);

        return invoice.Id;
    }
}
