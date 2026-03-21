using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Drivers;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Drivers.GetDrivers;

internal sealed class GetDriversQueryHandler(IApplicationDbContext context)
    : IQueryHandler<GetDriversQuery, PagedList<DriverResponse>>
{
    public async Task<Result<PagedList<DriverResponse>>> Handle(GetDriversQuery query, CancellationToken cancellationToken)
    {
        IQueryable<Driver> driversQuery = context.Drivers;

        if (!string.IsNullOrWhiteSpace(query.SearchTerm))
        {
            driversQuery = driversQuery.Where(d =>
                d.FirstName.Contains(query.SearchTerm) ||
                d.LastName.Contains(query.SearchTerm) ||
                d.EmployeeNumber.Contains(query.SearchTerm) ||
                d.Email != null && d.Email.Contains(query.SearchTerm));
        }

        if (query.Status.HasValue)
        {
            driversQuery = driversQuery.Where(d => d.Status == query.Status.Value);
        }

        if (query.IsActive.HasValue)
        {
            driversQuery = driversQuery.Where(d => d.IsActive == query.IsActive.Value);
        }

        int totalCount = await driversQuery.CountAsync(cancellationToken);

        List<DriverResponse> drivers = await driversQuery
            .OrderBy(d => d.LastName)
            .ThenBy(d => d.FirstName)
            .Skip((query.Page - 1) * query.PageSize)
            .Take(query.PageSize)
            .Select(d => new DriverResponse(
                d.Id,
                d.EmployeeNumber,
                d.FirstName,
                d.LastName,
                d.PhoneNumber,
                d.Email,
                d.LicenceNumber,
                d.LicenceExpiryDate,
                d.NationalityCode,
                d.SponsorName,
                (int)d.Status,
                d.IsActive,
                d.CreatedAt,
                d.UpdatedAt))
            .ToListAsync(cancellationToken);

        return new PagedList<DriverResponse>(drivers, totalCount, query.Page, query.PageSize);
    }
}
