using Domain.Billing;
using Domain.Clients.Enums;
using SharedKernel;

namespace Domain.Clients;

public sealed class Client : Entity
{
    public Guid Id { get; set; }
    public string ClientCode { get; set; } = string.Empty;
    public string CompanyName { get; set; } = string.Empty;
    public string ContactPersonName { get; set; } = string.Empty;
    public string ContactEmail { get; set; } = string.Empty;
    public string ContactPhone { get; set; } = string.Empty;
    public string BillingAddress { get; set; } = string.Empty;
    public string? TaxRegistrationNumber { get; set; }
    public int PaymentTermsDays { get; set; }
    public string CurrencyCode { get; set; } = "QAR";
    public bool IsActive { get; set; } = true;

    // Navigation Properties
    public ICollection<ClientPortalUser> PortalUsers { get; set; } = [];
    public ICollection<Quotation> Quotations { get; set; } = [];
    public ICollection<Invoice> Invoices { get; set; } = [];
    public ICollection<OutstandingInvoiceReport> OutstandingReports { get; set; } = [];
}
