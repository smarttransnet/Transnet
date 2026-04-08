using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Billing;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Quotations.Commands.UpdateQuotation;

internal sealed class UpdateQuotationCommandHandler(IApplicationDbContext dbContext)
    : ICommandHandler<UpdateQuotationCommand>
{
    public async Task<Result> Handle(UpdateQuotationCommand request, CancellationToken cancellationToken)
    {
        Quotation? quotation = await dbContext.Quotations
            .Include(q => q.LineItems)
            .FirstOrDefaultAsync(q => q.Id == request.Id, cancellationToken);

        if (quotation is null)
        {
            return Result.Failure(Error.NotFound("Quotation.NotFound", "The quotation was not found."));
        }

        quotation.ValidUntil = request.ValidUntil;
        quotation.Status = request.Status;
        quotation.Notes = request.Notes;
        quotation.TermsAndConditions = request.TermsAndConditions;

        // Update line items
        foreach (var itemCommand in request.LineItems)
        {
            if (itemCommand.Id.HasValue)
            {
                var existingItem = quotation.LineItems.FirstOrDefault(i => i.Id == itemCommand.Id!.Value);
                if (existingItem != null)
                {
                    UpdateItem(existingItem, itemCommand);
                }
                else
                {
                    AddItem(quotation, itemCommand);
                }
            }
            else
            {
                AddItem(quotation, itemCommand);
            }
        }

        // Remove old items
        var itemIdsToKeep = request.LineItems.Where(i => i.Id.HasValue).Select(i => i.Id!.Value).ToList();
        var itemsToRemove = quotation.LineItems.Where(i => !itemIdsToKeep.Contains(i.Id) && i.Id != Guid.Empty).ToList();
        foreach (var item in itemsToRemove)
        {
            quotation.LineItems.Remove(item);
            dbContext.QuotationLineItems.Remove(item); // Explicit deletion to prevent orphaning
        }

        // Recalculate totals
        quotation.SubTotalQAR = quotation.LineItems.Sum(i => i.LineTotalQAR);
        quotation.TaxAmountQAR = quotation.LineItems.Sum(i => i.LineTotalQAR * (i.TaxPercent / 100));
        quotation.TotalQAR = quotation.SubTotalQAR + quotation.TaxAmountQAR;

        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }

    private static void UpdateItem(QuotationLineItem item, UpdateQuotationLineItemCommand command)
    {
        item.Description = command.Description;
        item.ServiceType = command.ServiceType;
        item.Quantity = command.Quantity;
        item.UnitPriceQAR = command.UnitPriceQAR;
        item.DiscountPercent = command.DiscountPercent;
        item.TaxPercent = command.TaxPercent;
        item.SortOrder = command.SortOrder;
        item.LineTotalQAR = command.Quantity * command.UnitPriceQAR * (1 - command.DiscountPercent / 100);
    }

    private static void AddItem(Quotation quotation, UpdateQuotationLineItemCommand command)
    {
        quotation.LineItems.Add(new QuotationLineItem
        {
            Id = Guid.NewGuid(),
            QuotationId = quotation.Id,
            Description = command.Description,
            ServiceType = command.ServiceType,
            Quantity = command.Quantity,
            UnitPriceQAR = command.UnitPriceQAR,
            DiscountPercent = command.DiscountPercent,
            TaxPercent = command.TaxPercent,
            SortOrder = command.SortOrder,
            LineTotalQAR = command.Quantity * command.UnitPriceQAR * (1 - command.DiscountPercent / 100)
        });
    }
}
