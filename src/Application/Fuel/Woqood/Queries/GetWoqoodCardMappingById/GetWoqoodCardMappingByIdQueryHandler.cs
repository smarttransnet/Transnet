using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Fuel.Woqood.Queries.GetWoqoodCardMappingById;

internal sealed class GetWoqoodCardMappingByIdQueryHandler(IApplicationDbContext dbContext)
    : IQueryHandler<GetWoqoodCardMappingByIdQuery, WoqoodCardMappingResponse>
{
    public async Task<Result<WoqoodCardMappingResponse>> Handle(GetWoqoodCardMappingByIdQuery request, CancellationToken cancellationToken)
    {
        var mapping = await dbContext.WoqoodCardMappings
            .AsNoTracking()
            .Include(m => m.Vehicle)
            .Where(m => m.Id == request.Id)
            .Select(m => new WoqoodCardMappingResponse(
                m.Id,
                m.WoqoodCardNumber,
                m.VehicleId,
                m.DriverId,
                m.CardHolderName,
                m.IsActive,
                m.Notes))
            .FirstOrDefaultAsync(cancellationToken);

        if (mapping is null)
        {
            return Result.Failure<WoqoodCardMappingResponse>(Error.NotFound("WoqoodCardMapping.NotFound", $"The card mapping with ID {request.Id} was not found."));
        }

        return mapping;
    }
}
