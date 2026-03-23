using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Billing;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Quotations.Commands.DeleteQuotation;

internal sealed class DeleteQuotationCommandHandler(IApplicationDbContext dbContext)
    : ICommandHandler<DeleteQuotationCommand>
{
    public async Task<Result> Handle(DeleteQuotationCommand request, CancellationToken cancellationToken)
    {
        Quotation? quotation = await dbContext.Quotations
            .FirstOrDefaultAsync(q => q.Id == request.Id, cancellationToken);

        if (quotation is null)
        {
            return Result.Failure(Error.NotFound("Quotation.NotFound", "The quotation was not found."));
        }

        dbContext.Quotations.Remove(quotation);

        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
