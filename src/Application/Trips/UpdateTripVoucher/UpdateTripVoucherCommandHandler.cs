using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Trips;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Trips.UpdateTripVoucher;

internal sealed class UpdateTripVoucherCommandHandler : ICommandHandler<UpdateTripVoucherCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateTripVoucherCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result> Handle(UpdateTripVoucherCommand request, CancellationToken cancellationToken)
    {
        TripVoucher? voucher = await _context.TripVouchers
            .FirstOrDefaultAsync(v => v.TripId == request.TripId, cancellationToken);

        if (voucher is null)
        {
            return Result.Failure(Error.NotFound("TripVoucher.NotFound", $"Trip voucher for trip {request.TripId} was not found."));
        }

        voucher.VoucherNumber = request.VoucherNumber;
        voucher.VoucherDate = DateTime.SpecifyKind(request.VoucherDate, DateTimeKind.Utc);
        voucher.Notes = request.Notes;

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
