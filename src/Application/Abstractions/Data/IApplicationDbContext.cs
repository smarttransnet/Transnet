using Domain.Assets;
using Domain.Drivers;
using Domain.Inspections;
using Domain.Todos;
using Domain.Trips;
using Domain.Users;
using Domain.Billing;
using Domain.Clients;
using Domain.WorkOrders;
using Domain.Fuel;
using Domain.Payroll;
using Domain.Reports;
using Microsoft.EntityFrameworkCore;

namespace Application.Abstractions.Data;

public interface IApplicationDbContext
{
    DbSet<User> Users { get; }
    DbSet<TodoItem> TodoItems { get; }

    // Assets
    DbSet<Vehicle> Vehicles { get; }
    DbSet<VehicleCategory> VehicleCategories { get; }
    DbSet<Trailer> Trailers { get; }
    DbSet<AssetLocation> AssetLocations { get; }

    // Inspections
    DbSet<InspectionChecklist> InspectionChecklists { get; }
    DbSet<ChecklistItem> ChecklistItems { get; }
    DbSet<VehicleInspection> VehicleInspections { get; }
    DbSet<InspectionResult> InspectionResults { get; }
    DbSet<InspectionPhoto> InspectionPhotos { get; }

    // Work Orders
    DbSet<WorkOrder> WorkOrders { get; }
    DbSet<WorkOrderItem> WorkOrderItems { get; }
    DbSet<WorkOrderStatusHistory> WorkOrderStatusHistories { get; }

    // Trips
    DbSet<Trip> Trips { get; }
    DbSet<TripStop> TripStops { get; }
    DbSet<TripHalt> TripHalts { get; }
    DbSet<TripVoucher> TripVouchers { get; }
    DbSet<TripPodUpload> TripPodUploads { get; }
    DbSet<TripStatusHistory> TripStatusHistories { get; }
    DbSet<ImportBatch> ImportBatches { get; }
    DbSet<CustomFieldDefinition> CustomFieldDefinitions { get; }
    DbSet<TripCustomField> TripCustomFields { get; }

    // Module 04: Fuel & Cost Tracking
    DbSet<WoqoodImportBatch> WoqoodImportBatches { get; }
    DbSet<WoqoodFuelTransaction> WoqoodFuelTransactions { get; }
    DbSet<WoqoodCardMapping> WoqoodCardMappings { get; }
    DbSet<FuelCostAllocation> FuelCostAllocations { get; }
    DbSet<VehicleFuelSummary> VehicleFuelSummaries { get; }
    DbSet<DriverSalaryRecord> DriverSalaryRecords { get; }
    DbSet<SalaryExpenseLine> SalaryExpenseLines { get; }
    DbSet<DriverCommissionItem> DriverCommissionItems { get; }
    DbSet<MonthlyExpenseReport> MonthlyExpenseReports { get; }
    DbSet<ExpenseReportLineItem> ExpenseReportLineItems { get; }

    // Module 05: Client Billing and Invoicing
    DbSet<Client> Clients { get; }
    DbSet<ClientPortalUser> ClientPortalUsers { get; }
    DbSet<Quotation> Quotations { get; }
    DbSet<QuotationLineItem> QuotationLineItems { get; }
    DbSet<Invoice> Invoices { get; }
    DbSet<InvoiceLineItem> InvoiceLineItems { get; }
    DbSet<InvoiceTripLink> InvoiceTripLinks { get; }
    DbSet<InvoicePayment> InvoicePayments { get; }
    DbSet<InvoiceReportFormat> InvoiceReportFormats { get; }
    DbSet<ReportFormatColumn> ReportFormatColumns { get; }
    DbSet<OutstandingInvoiceReport> OutstandingInvoiceReports { get; }
    DbSet<OutstandingInvoiceSnapshot> OutstandingInvoiceSnapshots { get; }
    DbSet<InvoiceReminderLog> InvoiceReminderLogs { get; }

    // Drivers
    DbSet<Driver> Drivers { get; }
    DbSet<DriverAuthCredential> DriverAuthCredentials { get; }
    DbSet<DriverAttendanceLog> DriverAttendanceLogs { get; }
    DbSet<DriverExpense> DriverExpenses { get; }
    DbSet<DriverTripAssignment> DriverTripAssignments { get; }
    DbSet<DriverLocationUpdate> DriverLocationUpdates { get; }
    DbSet<DriverGpsLog> DriverGpsLogs { get; }
    DbSet<DriverDocument> DriverDocuments { get; }
    DbSet<DriverNotification> DriverNotifications { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
