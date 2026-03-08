using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Billing;
using Domain.Billing.Enums;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Quotations.Commands.ConvertQuotationToInvoice;

internal sealed class ConvertQuotationToInvoiceCommandHandler(
    IApplicationDbContext dbContext)
    : ICommandHandler<ConvertQuotationToInvoiceCommand, Guid>
{
    public async Task<Result<Guid>> Handle(ConvertQuotationToInvoiceCommand request, CancellationToken cancellationToken)
    {
        Quotation? quotation = await dbContext.Quotations
            .Include(q => q.LineItems)
            .FirstOrDefaultAsync(q => q.Id == request.QuotationId, cancellationToken);

        if (quotation is null)
        {
            return Result.Failure<Guid>(Error.NotFound("Quotation.NotFound", "The specified quotation was not found."));
        }

        if (quotation.Status == QuotationStatus.ConvertedToInvoice)
        {
            return Result.Failure<Guid>(Error.Problem("Quotation.AlreadyConverted", "This quotation has already been converted to an invoice."));
        }

        string datePrefix = DateTime.UtcNow.ToString("yyyyMM", System.Globalization.CultureInfo.InvariantCulture);
        int count = await dbContext.Invoices.CountAsync(i => i.InvoiceNumber.StartsWith($"INV-{datePrefix}"), cancellationToken);
        string invoiceNumber = $"INV-{datePrefix}-{(count + 1).ToString(System.Globalization.CultureInfo.InvariantCulture).PadLeft(4, '0')}";

        var invoice = new Invoice
        {
            Id = Guid.NewGuid(),
            InvoiceNumber = invoiceNumber,
            ClientId = quotation.ClientId,
            QuotationId = quotation.Id,
            IssuedByUserId = request.IssuedByUserId,
            IssuedAt = DateTime.UtcNow,
            DueDate = request.DueDate,
            Status = InvoiceStatus.Draft,
            SubTotalQAR = quotation.SubTotalQAR,
            TaxAmountQAR = quotation.TaxAmountQAR,
            TotalQAR = quotation.TotalQAR,
            PaidAmountQAR = 0m,
            OutstandingAmountQAR = quotation.TotalQAR,
            Notes = quotation.Notes
        };

        foreach (Domain.Billing.QuotationLineItem qItem in quotation.LineItems)
        {
            invoice.LineItems.Add(new InvoiceLineItem
            {
                Id = Guid.NewGuid(),
                InvoiceId = invoice.Id,
                Description = qItem.Description,
                ServiceType = qItem.ServiceType,
                Quantity = qItem.Quantity,
                UnitPriceQAR = qItem.UnitPriceQAR,
                DiscountPercent = qItem.DiscountPercent,
                TaxPercent = qItem.TaxPercent,
                LineTotalQAR = qItem.LineTotalQAR,
                SortOrder = qItem.SortOrder
            });
        }

        quotation.Status = QuotationStatus.ConvertedToInvoice;
        quotation.ConvertedToInvoiceId = invoice.Id;

        dbContext.Invoices.Add(invoice);
        await dbContext.SaveChangesAsync(cancellationToken);

        return invoice.Id;
    }
}
