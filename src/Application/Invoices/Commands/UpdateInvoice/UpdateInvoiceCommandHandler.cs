using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Billing;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Invoices.Commands.UpdateInvoice;

internal sealed class UpdateInvoiceCommandHandler(IApplicationDbContext dbContext)
    : ICommandHandler<UpdateInvoiceCommand>
{
    public async Task<Result> Handle(UpdateInvoiceCommand request, CancellationToken cancellationToken)
    {
        Invoice? invoice = await dbContext.Invoices
            .Include(i => i.LineItems)
            .FirstOrDefaultAsync(i => i.Id == request.Id, cancellationToken);

        if (invoice is null)
        {
            return Result.Failure(Error.NotFound("Invoice.NotFound", "The invoice was not found."));
        }

        invoice.DueDate = request.DueDate;
        invoice.Status = request.Status;
        invoice.Notes = request.Notes;
        invoice.ReportFormatId = request.ReportFormatId;

        // Update line items
        foreach (var itemCommand in request.LineItems)
        {
            if (itemCommand.Id.HasValue)
            {
                var existingItem = invoice.LineItems.FirstOrDefault(i => i.Id == itemCommand.Id!.Value);
                if (existingItem != null)
                {
                    UpdateItem(existingItem, itemCommand);
                }
                else
                {
                    AddItem(invoice, itemCommand);
                }
            }
            else
            {
                AddItem(invoice, itemCommand);
            }
        }

        // Remove old items
        var itemIdsToKeep = request.LineItems.Where(i => i.Id.HasValue).Select(i => i.Id!.Value).ToList();
        var itemsToRemove = invoice.LineItems.Where(i => !itemIdsToKeep.Contains(i.Id) && i.Id != Guid.Empty).ToList();
        foreach (var item in itemsToRemove)
        {
            invoice.LineItems.Remove(item);
            dbContext.InvoiceLineItems.Remove(item); // Explicit deletion to prevent orphaning
        }

        // Recalculate totals
        invoice.SubTotalQAR = invoice.LineItems.Sum(i => i.LineTotalQAR);
        invoice.TaxAmountQAR = invoice.LineItems.Sum(i => i.LineTotalQAR * (i.TaxPercent / 100));
        invoice.TotalQAR = invoice.SubTotalQAR + invoice.TaxAmountQAR;
        invoice.OutstandingAmountQAR = invoice.TotalQAR - invoice.PaidAmountQAR;

        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }

    private static void UpdateItem(InvoiceLineItem item, UpdateInvoiceLineItemCommand command)
    {
        item.Description = command.Description;
        item.ServiceType = command.ServiceType;
        item.Quantity = command.Quantity;
        item.UnitPriceQAR = command.UnitPriceQAR;
        item.DiscountPercent = command.DiscountPercent;
        item.TaxPercent = command.TaxPercent;
        item.SortOrder = command.SortOrder;
        item.TripId = command.TripId;
        item.LineTotalQAR = command.Quantity * command.UnitPriceQAR * (1 - command.DiscountPercent / 100);
    }

    private static void AddItem(Invoice invoice, UpdateInvoiceLineItemCommand command)
    {
        invoice.LineItems.Add(new InvoiceLineItem
        {
            Id = Guid.NewGuid(),
            InvoiceId = invoice.Id,
            Description = command.Description,
            ServiceType = command.ServiceType,
            Quantity = command.Quantity,
            UnitPriceQAR = command.UnitPriceQAR,
            DiscountPercent = command.DiscountPercent,
            TaxPercent = command.TaxPercent,
            SortOrder = command.SortOrder,
            TripId = command.TripId,
            LineTotalQAR = command.Quantity * command.UnitPriceQAR * (1 - command.DiscountPercent / 100)
        });
    }
}
