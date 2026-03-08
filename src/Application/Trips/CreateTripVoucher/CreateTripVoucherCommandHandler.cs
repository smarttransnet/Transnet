using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Trips;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Trips.CreateTripVoucher;

internal sealed class CreateTripVoucherCommandHandler : ICommandHandler<CreateTripVoucherCommand, Guid>
{
    private readonly IApplicationDbContext _context;

    public CreateTripVoucherCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<Guid>> Handle(CreateTripVoucherCommand request, CancellationToken cancellationToken)
    {
        bool tripExists = await _context.Trips.AnyAsync(t => t.Id == request.TripId, cancellationToken);
        if (!tripExists)
        {
            return Result.Failure<Guid>(TripErrors.NotFound(request.TripId));
        }

        bool voucherExists = await _context.TripVouchers.AnyAsync(v => v.TripId == request.TripId, cancellationToken);
        if (voucherExists)
        {
            return Result.Failure<Guid>(Error.Conflict("TripVoucher.AlreadyExists", $"A voucher already exists for trip {request.TripId}."));
        }

        TripVoucher voucher = new()
        {
            Id = Guid.NewGuid(),
            TripId = request.TripId,
            VoucherNumber = request.VoucherNumber,
            VoucherDate = DateTime.SpecifyKind(request.VoucherDate, DateTimeKind.Utc),
            Notes = request.Notes,
            CreatedByUserId = request.CreatedByUserId,
            CreatedAt = DateTime.UtcNow
        };

        _context.TripVouchers.Add(voucher);
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success(voucher.Id);
    }
}
