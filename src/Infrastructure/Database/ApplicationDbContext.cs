using Application.Abstractions.Data;
using Domain.Assets;
using Domain.Drivers;
using Domain.Inspections;
using Domain.Todos;
using Domain.Trips;
using Domain.Users;
using Domain.Billing;
using Domain.Clients;
using Domain.Fuel;
using Domain.Payroll;
using Domain.Reports;
using Domain.WorkOrders;
using Infrastructure.DomainEvents;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Infrastructure.Database;

public sealed class ApplicationDbContext(
    DbContextOptions<ApplicationDbContext> options,
    IDomainEventsDispatcher domainEventsDispatcher)
    : DbContext(options), IApplicationDbContext
{
    public DbSet<User> Users { get; set; }

    public DbSet<TodoItem> TodoItems { get; set; }

    // Drivers
    public DbSet<Driver> Drivers { get; set; }
    public DbSet<DriverAuthCredential> DriverAuthCredentials { get; set; }
    public DbSet<DriverAttendanceLog> DriverAttendanceLogs { get; set; }
    public DbSet<DriverExpense> DriverExpenses { get; set; }
    public DbSet<DriverLocationUpdate> DriverLocationUpdates { get; set; }
    public DbSet<DriverNotification> DriverNotifications { get; set; }
    public DbSet<DriverDocument> DriverDocuments { get; set; }
    public DbSet<DriverGpsLog> DriverGpsLogs { get; set; }
    public DbSet<DriverTripAssignment> DriverTripAssignments { get; set; }

    // Assets
    public DbSet<Vehicle> Vehicles { get; set; }
    public DbSet<VehicleCategory> VehicleCategories { get; set; }
    public DbSet<Trailer> Trailers { get; set; }
    public DbSet<AssetLocation> AssetLocations { get; set; }

    // Inspections
    public DbSet<InspectionChecklist> InspectionChecklists { get; set; }
    public DbSet<ChecklistItem> ChecklistItems { get; set; }
    public DbSet<VehicleInspection> VehicleInspections { get; set; }
    public DbSet<InspectionResult> InspectionResults { get; set; }
    public DbSet<InspectionPhoto> InspectionPhotos { get; set; }

    // Work Orders
    public DbSet<WorkOrder> WorkOrders { get; set; }
    public DbSet<WorkOrderItem> WorkOrderItems { get; set; }
    public DbSet<WorkOrderStatusHistory> WorkOrderStatusHistories { get; set; }

    // Trips
    public DbSet<Trip> Trips { get; set; }
    public DbSet<TripStop> TripStops { get; set; }
    public DbSet<TripHalt> TripHalts { get; set; }
    public DbSet<TripVoucher> TripVouchers { get; set; }
    public DbSet<TripPodUpload> TripPodUploads { get; set; }
    public DbSet<TripStatusHistory> TripStatusHistories { get; set; }
    public DbSet<ImportBatch> ImportBatches { get; set; }
    public DbSet<CustomFieldDefinition> CustomFieldDefinitions { get; set; }
    public DbSet<TripCustomField> TripCustomFields { get; set; }

    // Module 04: Fuel & Cost Tracking
    public DbSet<WoqoodImportBatch> WoqoodImportBatches => Set<WoqoodImportBatch>();
    public DbSet<WoqoodFuelTransaction> WoqoodFuelTransactions => Set<WoqoodFuelTransaction>();
    public DbSet<WoqoodCardMapping> WoqoodCardMappings => Set<WoqoodCardMapping>();
    public DbSet<FuelCostAllocation> FuelCostAllocations => Set<FuelCostAllocation>();
    public DbSet<VehicleFuelSummary> VehicleFuelSummaries => Set<VehicleFuelSummary>();
    public DbSet<DriverSalaryRecord> DriverSalaryRecords => Set<DriverSalaryRecord>();
    public DbSet<SalaryExpenseLine> SalaryExpenseLines { get; set; }
    public DbSet<DriverCommissionItem> DriverCommissionItems => Set<DriverCommissionItem>();
    public DbSet<MonthlyExpenseReport> MonthlyExpenseReports => Set<MonthlyExpenseReport>();
    public DbSet<ExpenseReportLineItem> ExpenseReportLineItems { get; set; }

    // Module 05: Client Billing and Invoicing
    public DbSet<Client> Clients => Set<Client>();
    public DbSet<ClientPortalUser> ClientPortalUsers => Set<ClientPortalUser>();
    public DbSet<Quotation> Quotations => Set<Quotation>();
    public DbSet<QuotationLineItem> QuotationLineItems => Set<QuotationLineItem>();
    public DbSet<Invoice> Invoices => Set<Invoice>();
    public DbSet<InvoiceLineItem> InvoiceLineItems => Set<InvoiceLineItem>();
    public DbSet<InvoiceTripLink> InvoiceTripLinks => Set<InvoiceTripLink>();
    public DbSet<InvoicePayment> InvoicePayments => Set<InvoicePayment>();
    public DbSet<InvoiceReportFormat> InvoiceReportFormats => Set<InvoiceReportFormat>();
    public DbSet<ReportFormatColumn> ReportFormatColumns => Set<ReportFormatColumn>();
    public DbSet<OutstandingInvoiceReport> OutstandingInvoiceReports => Set<OutstandingInvoiceReport>();
    public DbSet<OutstandingInvoiceSnapshot> OutstandingInvoiceSnapshots => Set<OutstandingInvoiceSnapshot>();
    public DbSet<InvoiceReminderLog> InvoiceReminderLogs => Set<InvoiceReminderLog>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

        modelBuilder.HasDefaultSchema(Schemas.Default);

        // Drivers

        // Assets

        // Inspections

        // Work Orders

        // Trips

        // Fuel & Cost

        // Billing
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // When should you publish domain events?
        //
        // 1. BEFORE calling SaveChangesAsync
        //     - domain events are part of the same transaction
        //     - immediate consistency
        // 2. AFTER calling SaveChangesAsync
        //     - domain events are a separate transaction
        //     - eventual consistency
        //     - handlers can fail

        int result = await base.SaveChangesAsync(cancellationToken);

        await PublishDomainEventsAsync();

        return result;
    }

    private async Task PublishDomainEventsAsync()
    {
        var domainEvents = ChangeTracker
            .Entries<Entity>()
            .Select(entry => entry.Entity)
            .SelectMany(entity =>
            {
                List<IDomainEvent> domainEvents = entity.DomainEvents;

                entity.ClearDomainEvents();

                return domainEvents;
            })
            .ToList();

        await domainEventsDispatcher.DispatchAsync(domainEvents);
    }
}
