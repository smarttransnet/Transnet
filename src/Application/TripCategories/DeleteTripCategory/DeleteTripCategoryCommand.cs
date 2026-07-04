using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Microsoft.EntityFrameworkCore;
using SharedKernel;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.TripCategories.DeleteTripCategory;

public sealed record DeleteTripCategoryCommand(Guid CategoryId) : ICommand;

internal sealed class DeleteTripCategoryCommandHandler(
    IApplicationDbContext dbContext
) : ICommandHandler<DeleteTripCategoryCommand>
{
    public async Task<Result> Handle(
        DeleteTripCategoryCommand request,
        CancellationToken cancellationToken
    ) {
        var category = await dbContext.TripCategories
            .Include(c => c.CategoryMaterials)
            .FirstOrDefaultAsync(c => c.Id == request.CategoryId, cancellationToken);

        if (category == null)
        {
            return Result.Failure(Error.NotFound(
                "TripCategory.NotFound",
                $"Trip category with ID '{request.CategoryId}' was not found."
            ));
        }

        // Hard delete mappings first
        dbContext.TripCategoryMaterials.RemoveRange(category.CategoryMaterials);
        
        // Hard delete the category
        dbContext.TripCategories.Remove(category);

        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
