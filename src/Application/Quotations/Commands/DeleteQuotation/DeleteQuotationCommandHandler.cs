using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Billing;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Quotations.Commands.DeleteQuotation;

internal sealed class DeleteQuotationCommandHandler(IApplicationDbContext context)
    : ICommandHandler<DeleteQuotationCommand>
{
    public async Task<Result> Handle(DeleteQuotationCommand command, CancellationToken cancellationToken)
    {
        Quotation? quotation = await context.Quotations
            .Include(q => q.LineItems)
            .FirstOrDefaultAsync(q => q.Id == command.Id, cancellationToken);

        if (quotation is null)
        {
            return Result.Failure(Error.NotFound("Quotations.NotFound", $"The quotation with the Id = '{command.Id}' was not found"));
        }

        context.Quotations.Remove(quotation);

        await context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
