using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Drivers.DeleteDriver;

internal sealed class DeleteDriverCommandHandler(IApplicationDbContext context)
    : ICommandHandler<DeleteDriverCommand>
{
    public async Task<Result> Handle(DeleteDriverCommand request, CancellationToken cancellationToken)
    {
        var driver = await context.Drivers
            .FirstOrDefaultAsync(d => d.Id == request.Id, cancellationToken);

        if (driver is null)
        {
            return Result.Failure(Error.NotFound(
                "Driver.NotFound",
                $"The driver with the ID '{request.Id}' was not found."));
        }

        // Soft delete logic as requested by user
        driver.IsActive = false;
        driver.UpdatedAt = DateTime.UtcNow;

        await context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
