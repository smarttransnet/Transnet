using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Fuel;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Fuel.Woqood.Commands.UpsertWoqoodCardMapping;

internal sealed class UpsertWoqoodCardMappingCommandHandler(
    IApplicationDbContext dbContext)
    : ICommandHandler<UpsertWoqoodCardMappingCommand, Guid>
{
    public async Task<Result<Guid>> Handle(UpsertWoqoodCardMappingCommand request, CancellationToken cancellationToken)
    {
        WoqoodCardMapping? existing = await dbContext.WoqoodCardMappings
            .FirstOrDefaultAsync(m => m.WoqoodCardNumber == request.WoqoodCardNumber, cancellationToken);

        if (existing is not null)
        {
            existing.VehicleId = request.VehicleId;
            existing.DriverId = request.DriverId;
            existing.CardHolderName = request.CardHolderName;
            existing.Notes = request.Notes;
            existing.MappedAt = DateTime.UtcNow;
            existing.MappedByUserId = request.UserId;
            existing.IsActive = true;
            
            await dbContext.SaveChangesAsync(cancellationToken);
            return existing.Id;
        }

        var newMapping = new WoqoodCardMapping
        {
            Id = Guid.NewGuid(),
            WoqoodCardNumber = request.WoqoodCardNumber,
            VehicleId = request.VehicleId,
            DriverId = request.DriverId,
            CardHolderName = request.CardHolderName,
            Notes = request.Notes,
            MappedAt = DateTime.UtcNow,
            MappedByUserId = request.UserId,
            IsActive = true
        };

        dbContext.WoqoodCardMappings.Add(newMapping);
        await dbContext.SaveChangesAsync(cancellationToken);

        return newMapping.Id;
    }
}
