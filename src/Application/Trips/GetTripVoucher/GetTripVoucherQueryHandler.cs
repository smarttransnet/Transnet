using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Trips.Common;
using Domain.Trips;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Trips.GetTripVoucher;

internal sealed class GetTripVoucherQueryHandler : IQueryHandler<GetTripVoucherQuery, TripVoucherResponse>
{
    private readonly IApplicationDbContext _context;

    public GetTripVoucherQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<TripVoucherResponse>> Handle(GetTripVoucherQuery request, CancellationToken cancellationToken)
    {
        TripVoucher? voucher = await _context.TripVouchers
            .Include(v => v.CustomFields)
            .FirstOrDefaultAsync(v => v.TripId == request.TripId, cancellationToken);

        if (voucher is null)
        {
            return Result.Failure<TripVoucherResponse>(Error.NotFound("TripVoucher.NotFound", $"Voucher for trip {request.TripId} was not found."));
        }

        TripVoucherResponse response = new(
            voucher.Id,
            voucher.TripId,
            voucher.VoucherNumber,
            voucher.VoucherDate,
            voucher.Notes,
            voucher.CreatedByUserId,
            voucher.CreatedAt,
            voucher.CustomFields.Select(cf => new TripCustomFieldResponse(
                cf.Id,
                cf.TripId,
                cf.TripVoucherId,
                cf.FieldDefinitionId,
                cf.Value)).ToList());

        return Result.Success(response);
    }
}
