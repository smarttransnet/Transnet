using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Microsoft.EntityFrameworkCore;
using SharedKernel;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.TripCategories.DeleteTripCategory;

public sealed record DeleteTripCategoryCommand(Guid Id) : ICommand;

internal sealed class DeleteTripCategoryCommandHandler(
    IApplicationDbContext dbContext
) : ICommandHandler<DeleteTripCategoryCommand>
{
    public async Task<Result> Handle(
        DeleteTripCategoryCommand request,
        CancellationToken cancellationToken
    ) {
        var mapping = await dbContext.TripCategoryMaterials
            .FirstOrDefaultAsync(cm => cm.Id == request.Id, cancellationToken);

        if (mapping == null)
        {
            return Result.Failure(Error.NotFound(
                "TripCategoryMaterial.NotFound",
                $"Trip category material mapping with ID '{request.Id}' was not found."
            ));
        }

        // Hard delete
        dbContext.TripCategoryMaterials.Remove(mapping);

        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
