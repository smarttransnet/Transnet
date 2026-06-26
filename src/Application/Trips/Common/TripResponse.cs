using Domain.Trips.Enums;

namespace Application.Trips.Common;

public sealed record TripResponse(
    Guid Id,
    string TripNumber,
    Guid DriverId,
    Guid VehicleId,
    Guid? TrailerId,
    TripStatus Status,
    DateTime ScheduledStartAt,
    DateTime? ActualStartAt,
    DateTime? ActualEndAt,
    decimal? TotalDistanceKm,
    bool IsImported,
    Guid? ImportBatchId,
    string Origin,
    string Destination,
    DateTime? DriverConfirmedAt,
    DateTime? OfficeApprovedAt,
    Guid? OfficeApprovedByUserId,
    DateTime CreatedAt,
    DateTime UpdatedAt,
    string? DriverName = null,
    string? VehicleRegistrationNumber = null,
    string? ClientName = null,
    Guid? ClientId = null,
    string? ResponseVersion = null,

    string? VehiclePlateNumber = null,
    string? VehicleCategoryName = null,
    Guid? TripCategoryMaterialId = null,
    string? CategoryName = null,
    string? MaterialName = null,
    string? UomCode = null,
    decimal? Quantity = null,
    List<TripStatusHistoryResponse>? StatusHistory = null);
