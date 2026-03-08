using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Fuel.Woqood.Queries.GetWoqoodCardMappings;

internal sealed class GetWoqoodCardMappingsQueryHandler(
    IApplicationDbContext dbContext)
    : IQueryHandler<GetWoqoodCardMappingsQuery, IReadOnlyList<WoqoodCardMappingResponse>>
{
    public async Task<Result<IReadOnlyList<WoqoodCardMappingResponse>>> Handle(GetWoqoodCardMappingsQuery request, CancellationToken cancellationToken)
    {
        List<WoqoodCardMappingResponse> mappings = await dbContext.WoqoodCardMappings
            .OrderBy(m => m.WoqoodCardNumber)
            .Select(m => new WoqoodCardMappingResponse(
                m.Id,
                m.WoqoodCardNumber,
                m.VehicleId,
                m.DriverId,
                m.CardHolderName,
                m.IsActive,
                m.Notes
            ))
            .ToListAsync(cancellationToken);

        return mappings;
    }
}
