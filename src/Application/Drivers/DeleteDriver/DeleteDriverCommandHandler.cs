using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Drivers;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Drivers.DeleteDriver;

internal sealed class DeleteDriverCommandHandler(IApplicationDbContext dbContext)
    : ICommandHandler<DeleteDriverCommand>
{
    public async Task<Result> Handle(DeleteDriverCommand request, CancellationToken cancellationToken)
    {
        Driver? driver = await dbContext.Drivers
            .FirstOrDefaultAsync(d => d.Id == request.Id, cancellationToken);

        if (driver is null)
        {
            return Result.Failure(Error.NotFound("Driver.NotFound", "The driver was not found."));
        }

        dbContext.Drivers.Remove(driver);

        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
