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
        var invoice = await dbContext.Invoices
            .Include(i => i.LineItems)
            .FirstOrDefaultAsync(i => i.Id == request.Id, cancellationToken);

        if (invoice is null)
        {
            return Result.Failure(Error.NotFound("Invoice.NotFound", $"The invoice with ID {request.Id} was not found."));
        }

        invoice.DueDate = request.DueDate;
        invoice.Status = request.Status;
        invoice.Notes = request.Notes;

        // Handle items
        var existingItems = invoice.LineItems.ToList();
        var incomingItemIds = request.Items.Where(i => i.Id.HasValue).Select(i => i.Id!.Value).ToList();

        // Remove items
        foreach (var existingItem in existingItems)
        {
            if (!incomingItemIds.Contains(existingItem.Id))
            {
                dbContext.InvoiceLineItems.Remove(existingItem);
            }
        }

        // Update or add items
        foreach (var itemCommand in request.Items)
        {
            if (itemCommand.Id.HasValue)
            {
                var existingItem = existingItems.FirstOrDefault(i => i.Id == itemCommand.Id.Value);
                if (existingItem != null)
                {
                    existingItem.Description = itemCommand.Description;
                    existingItem.ServiceType = itemCommand.ServiceType;
                    existingItem.Quantity = itemCommand.Quantity;
                    existingItem.UnitPriceQAR = itemCommand.UnitPriceQAR;
                    existingItem.DiscountPercent = itemCommand.DiscountPercent;
                    existingItem.TaxPercent = itemCommand.TaxPercent;
                    existingItem.SortOrder = itemCommand.SortOrder;
                    existingItem.TripId = itemCommand.TripId;
                    
                    existingItem.LineTotalQAR = itemCommand.Quantity * itemCommand.UnitPriceQAR * (1 - itemCommand.DiscountPercent / 100) * (1 + itemCommand.TaxPercent / 100);
                }
            }
            else
            {
                var newItem = new InvoiceLineItem
                {
                    Id = Guid.NewGuid(),
                    InvoiceId = invoice.Id,
                    Description = itemCommand.Description,
                    ServiceType = itemCommand.ServiceType,
                    Quantity = itemCommand.Quantity,
                    UnitPriceQAR = itemCommand.UnitPriceQAR,
                    DiscountPercent = itemCommand.DiscountPercent,
                    TaxPercent = itemCommand.TaxPercent,
                    SortOrder = itemCommand.SortOrder,
                    TripId = itemCommand.TripId,
                    LineTotalQAR = itemCommand.Quantity * itemCommand.UnitPriceQAR * (1 - itemCommand.DiscountPercent / 100) * (1 + itemCommand.TaxPercent / 100)
                };
                dbContext.InvoiceLineItems.Add(newItem);
            }
        }

        // Recalculate invoice totals
        invoice.SubTotalQAR = invoice.LineItems.Sum(i => i.Quantity * i.UnitPriceQAR * (1 - i.DiscountPercent / 100));
        invoice.TotalQAR = invoice.LineItems.Sum(i => i.LineTotalQAR);
        invoice.TaxAmountQAR = invoice.TotalQAR - invoice.SubTotalQAR;
        invoice.OutstandingAmountQAR = invoice.TotalQAR - invoice.PaidAmountQAR;

        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
