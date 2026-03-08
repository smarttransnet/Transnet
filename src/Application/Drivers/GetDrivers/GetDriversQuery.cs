using Application.Abstractions.Messaging;
using Domain.Drivers;
using Domain.Drivers.Enums;
using SharedKernel;
namespace Application.Drivers.GetDrivers; public sealed record GetDriversQuery(string? SearchTerm, DriverStatus? Status, bool? IsActive, int Page = 1, int PageSize = 10) : IQuery<PagedList<DriverResponse>>;
