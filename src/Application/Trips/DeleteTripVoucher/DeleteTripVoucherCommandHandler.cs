using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Trips;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Trips.DeleteTripVoucher;

internal sealed class DeleteTripVoucherCommandHandler : ICommandHandler<DeleteTripVoucherCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteTripVoucherCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result> Handle(DeleteTripVoucherCommand request, CancellationToken cancellationToken)
    {
        TripVoucher? voucher = await _context.TripVouchers
            .FirstOrDefaultAsync(v => v.TripId == request.TripId, cancellationToken);

        if (voucher is null)
        {
            return Result.Failure(Error.NotFound("TripVoucher.NotFound", $"Trip voucher for trip {request.TripId} was not found."));
        }

        _context.TripVouchers.Remove(voucher);
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
