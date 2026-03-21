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
        var quotation = await dbContext.Quotations
            .Include(q => q.LineItems)
            .FirstOrDefaultAsync(q => q.Id == request.Id, cancellationToken);

        if (quotation is null)
        {
            return Result.Failure(Error.NotFound("Quotation.NotFound", $"The quotation with ID {request.Id} was not found."));
        }

        quotation.ValidUntil = request.ValidUntil;
        quotation.Status = request.Status;
        quotation.Notes = request.Notes;
        quotation.TermsAndConditions = request.TermsAndConditions;

        // Handle items
        var existingItems = quotation.LineItems.ToList();
        var incomingItemIds = request.Items.Where(i => i.Id.HasValue).Select(i => i.Id!.Value).ToList();

        // Remove items
        foreach (var existingItem in existingItems)
        {
            if (!incomingItemIds.Contains(existingItem.Id))
            {
                dbContext.QuotationLineItems.Remove(existingItem);
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
                    existingItem.LineTotalQAR = itemCommand.Quantity * itemCommand.UnitPriceQAR * (1 - itemCommand.DiscountPercent / 100) * (1 + itemCommand.TaxPercent / 100);
                }
            }
            else
            {
                var newItem = new QuotationLineItem
                {
                    Id = Guid.NewGuid(),
                    QuotationId = quotation.Id,
                    Description = itemCommand.Description,
                    ServiceType = itemCommand.ServiceType,
                    Quantity = itemCommand.Quantity,
                    UnitPriceQAR = itemCommand.UnitPriceQAR,
                    DiscountPercent = itemCommand.DiscountPercent,
                    TaxPercent = itemCommand.TaxPercent,
                    SortOrder = itemCommand.SortOrder,
                    LineTotalQAR = itemCommand.Quantity * itemCommand.UnitPriceQAR * (1 - itemCommand.DiscountPercent / 100) * (1 + itemCommand.TaxPercent / 100)
                };
                dbContext.QuotationLineItems.Add(newItem);
            }
        }

        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
