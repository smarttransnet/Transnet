using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.VehicleCategoryMappings.GetMappings;
using Microsoft.EntityFrameworkCore;
using SharedKernel;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.VehicleCategoryMappings.GetMappingById;

public sealed record GetVehicleCategoryMappingByIdQuery(Guid Id) : IQuery<VehicleCategoryMappingResponse>;

internal sealed class GetVehicleCategoryMappingByIdQueryHandler(IApplicationDbContext dbContext)
    : IQueryHandler<GetVehicleCategoryMappingByIdQuery, VehicleCategoryMappingResponse>
{
    public async Task<Result<VehicleCategoryMappingResponse>> Handle(
        GetVehicleCategoryMappingByIdQuery request,
        CancellationToken cancellationToken
    ) {
        var c = await dbContext.VehicleCategories
            .Include(c => c.VehicleCategoryUoms)
            .ThenInclude(m => m.Uom)
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

        if (c == null)
        {
            return Result.Failure<VehicleCategoryMappingResponse>(Error.NotFound(
                "VehicleCategory.NotFound",
                $"Vehicle category with ID '{request.Id}' was not found."
            ));
        }

        var response = new VehicleCategoryMappingResponse(
            c.Id,
            c.Name,
            c.IsActive,
            c.VehicleCategoryUoms
                .Where(m => m.IsActive)
                .Select(m => new VehicleCategoryUomResponse(
                    m.Id,
                    m.UOMId,
                    m.Uom!.UOMCode,
                    m.Uom.Description
                )).ToList()
        );

        return response;
    }
}
