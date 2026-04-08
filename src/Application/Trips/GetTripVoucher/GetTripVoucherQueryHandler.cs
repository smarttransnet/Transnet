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
        var voucherResult = await _context.TripVouchers
            .Where(v => v.TripId == request.TripId)
            .Include(v => v.CustomFields)
            .Select(v => new
            {
                Voucher = v,
                CreatedByUserName = _context.Users
                    .Where(u => u.Id == v.CreatedByUserId)
                    .Select(u => u.FirstName + " " + u.LastName)
                    .FirstOrDefault() ?? "System Administrator"
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (voucherResult is null)
        {
            return Result.Failure<TripVoucherResponse>(Error.NotFound("TripVoucher.NotFound", $"Voucher for trip {request.TripId} was not found."));
        }

        TripVoucherResponse response = new(
            voucherResult.Voucher.Id,
            voucherResult.Voucher.TripId,
            voucherResult.Voucher.VoucherNumber,
            voucherResult.Voucher.VoucherDate,
            voucherResult.Voucher.Notes,
            voucherResult.Voucher.CreatedByUserId,
            voucherResult.CreatedByUserName,
            voucherResult.Voucher.CreatedAt,
            voucherResult.Voucher.CustomFields.Select(cf => new TripCustomFieldResponse(
                cf.Id,
                cf.TripId,
                cf.TripVoucherId,
                cf.FieldDefinitionId,
                cf.Value)).ToList());

        return Result.Success(response);
    }
}
