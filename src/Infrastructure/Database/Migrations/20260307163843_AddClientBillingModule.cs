#pragma warning disable IDE0161, CA1861
using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddClientBillingModule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Clients",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    client_code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    company_name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    contact_person_name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    contact_email = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    contact_phone = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    billing_address = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    tax_registration_number = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    payment_terms_days = table.Column<int>(type: "int", nullable: false),
                    currency_code = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    is_active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_clients", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "InvoiceReportFormats",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    template_file_url = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    show_shipping_address = table.Column<bool>(type: "bit", nullable: false),
                    show_tax_breakdown = table.Column<bool>(type: "bit", nullable: false),
                    show_trip_details = table.Column<bool>(type: "bit", nullable: false),
                    column_configuration = table.Column<string>(type: "nvarchar(1500)", maxLength: 1500, nullable: false),
                    header_logo_url = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    footer_text = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    is_default = table.Column<bool>(type: "bit", nullable: false),
                    is_active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_invoice_report_formats", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "ClientPortalUsers",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    client_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    full_name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    email = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    password_hash = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    password_salt = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    role = table.Column<int>(type: "int", nullable: false),
                    is_active = table.Column<bool>(type: "bit", nullable: false),
                    last_login_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    refresh_token = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    refresh_token_expires_at = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_client_portal_users", x => x.id);
                    table.ForeignKey(
                        name: "fk_client_portal_users_clients_client_id",
                        column: x => x.client_id,
                        principalTable: "Clients",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OutstandingInvoiceReports",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    client_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    generated_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    period_month = table.Column<int>(type: "int", nullable: false),
                    period_year = table.Column<int>(type: "int", nullable: false),
                    total_outstanding_qar = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    invoice_count = table.Column<int>(type: "int", nullable: false),
                    oldest_invoice_date = table.Column<DateOnly>(type: "date", nullable: true),
                    delivery_status = table.Column<int>(type: "int", nullable: false),
                    sent_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    sent_to_email = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    exported_file_url = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_outstanding_invoice_reports", x => x.id);
                    table.ForeignKey(
                        name: "fk_outstanding_invoice_reports_clients_client_id",
                        column: x => x.client_id,
                        principalTable: "Clients",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Quotations",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    quotation_number = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    client_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    issued_by_user_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    issued_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    valid_until = table.Column<DateOnly>(type: "date", nullable: false),
                    status = table.Column<int>(type: "int", nullable: false),
                    sub_total_qar = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    tax_amount_qar = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    total_qar = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    notes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    terms_and_conditions = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    converted_to_invoice_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_quotations", x => x.id);
                    table.ForeignKey(
                        name: "fk_quotations_clients_client_id",
                        column: x => x.client_id,
                        principalTable: "Clients",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ReportFormatColumns",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    invoice_report_format_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    column_key = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    display_label = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    width_percent = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    is_visible = table.Column<bool>(type: "bit", nullable: false),
                    sort_order = table.Column<int>(type: "int", nullable: false),
                    format_pattern = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_report_format_columns", x => x.id);
                    table.ForeignKey(
                        name: "fk_report_format_columns_invoice_report_formats_invoice_report_format_id",
                        column: x => x.invoice_report_format_id,
                        principalTable: "InvoiceReportFormats",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Invoices",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    invoice_number = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    client_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    quotation_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    issued_by_user_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    issued_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    due_date = table.Column<DateOnly>(type: "date", nullable: false),
                    status = table.Column<int>(type: "int", nullable: false),
                    sub_total_qar = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    tax_amount_qar = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    total_qar = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    paid_amount_qar = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    outstanding_amount_qar = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    notes = table.Column<string>(type: "nvarchar(1500)", maxLength: 1500, nullable: true),
                    report_format_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_invoices", x => x.id);
                    table.ForeignKey(
                        name: "fk_invoices_clients_client_id",
                        column: x => x.client_id,
                        principalTable: "Clients",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_invoices_invoice_report_formats_report_format_id",
                        column: x => x.report_format_id,
                        principalTable: "InvoiceReportFormats",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_invoices_quotations_quotation_id",
                        column: x => x.quotation_id,
                        principalTable: "Quotations",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "QuotationLineItems",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    quotation_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    service_type = table.Column<int>(type: "int", nullable: false),
                    quantity = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    unit_price_qar = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    discount_percent = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    tax_percent = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    line_total_qar = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    sort_order = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_quotation_line_items", x => x.id);
                    table.ForeignKey(
                        name: "fk_quotation_line_items_quotations_quotation_id",
                        column: x => x.quotation_id,
                        principalTable: "Quotations",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InvoiceLineItems",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    invoice_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    trip_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    service_type = table.Column<int>(type: "int", nullable: false),
                    quantity = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    unit_price_qar = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    discount_percent = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    tax_percent = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    line_total_qar = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    sort_order = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_invoice_line_items", x => x.id);
                    table.ForeignKey(
                        name: "fk_invoice_line_items_invoices_invoice_id",
                        column: x => x.invoice_id,
                        principalTable: "Invoices",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_invoice_line_items_trips_trip_id",
                        column: x => x.trip_id,
                        principalTable: "trips",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "InvoicePayments",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    invoice_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    amount_qar = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    payment_method = table.Column<int>(type: "int", nullable: false),
                    payment_reference = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    payment_date = table.Column<DateOnly>(type: "date", nullable: false),
                    recorded_by_user_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    notes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_invoice_payments", x => x.id);
                    table.ForeignKey(
                        name: "fk_invoice_payments_invoices_invoice_id",
                        column: x => x.invoice_id,
                        principalTable: "Invoices",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InvoiceReminderLogs",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    invoice_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    sent_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    sent_to_email = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    reminder_type = table.Column<int>(type: "int", nullable: false),
                    delivery_status = table.Column<int>(type: "int", nullable: false),
                    delivery_error = table.Column<string>(type: "nvarchar(1500)", maxLength: 1500, nullable: true),
                    triggered_by_user_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    is_automated = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_invoice_reminder_logs", x => x.id);
                    table.ForeignKey(
                        name: "fk_invoice_reminder_logs_invoices_invoice_id",
                        column: x => x.invoice_id,
                        principalTable: "Invoices",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InvoiceTripLinks",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    invoice_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    trip_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    linked_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    linked_by_user_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    trip_completion_verified = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_invoice_trip_links", x => x.id);
                    table.ForeignKey(
                        name: "fk_invoice_trip_links_invoices_invoice_id",
                        column: x => x.invoice_id,
                        principalTable: "Invoices",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_invoice_trip_links_trips_trip_id",
                        column: x => x.trip_id,
                        principalTable: "trips",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OutstandingInvoiceSnapshots",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    outstanding_invoice_report_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    invoice_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    invoice_number = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    issued_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    due_date = table.Column<DateOnly>(type: "date", nullable: false),
                    original_amount_qar = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    outstanding_amount_qar = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    aging_days = table.Column<int>(type: "int", nullable: false),
                    aging_bucket = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_outstanding_invoice_snapshots", x => x.id);
                    table.ForeignKey(
                        name: "fk_outstanding_invoice_snapshots_invoices_invoice_id",
                        column: x => x.invoice_id,
                        principalTable: "Invoices",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_outstanding_invoice_snapshots_outstanding_invoice_reports_outstanding_invoice_report_id",
                        column: x => x.outstanding_invoice_report_id,
                        principalTable: "OutstandingInvoiceReports",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_client_portal_users_client_id",
                table: "ClientPortalUsers",
                column: "client_id");

            migrationBuilder.CreateIndex(
                name: "ix_client_portal_users_email",
                table: "ClientPortalUsers",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_clients_client_code",
                table: "Clients",
                column: "client_code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_clients_company_name",
                table: "Clients",
                column: "company_name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_invoice_line_items_invoice_id",
                table: "InvoiceLineItems",
                column: "invoice_id");

            migrationBuilder.CreateIndex(
                name: "ix_invoice_line_items_trip_id",
                table: "InvoiceLineItems",
                column: "trip_id");

            migrationBuilder.CreateIndex(
                name: "ix_invoice_payments_invoice_id",
                table: "InvoicePayments",
                column: "invoice_id");

            migrationBuilder.CreateIndex(
                name: "ix_invoice_payments_payment_date",
                table: "InvoicePayments",
                column: "payment_date");

            migrationBuilder.CreateIndex(
                name: "ix_invoice_reminder_logs_invoice_id",
                table: "InvoiceReminderLogs",
                column: "invoice_id");

            migrationBuilder.CreateIndex(
                name: "ix_invoice_reminder_logs_sent_at",
                table: "InvoiceReminderLogs",
                column: "sent_at");

            migrationBuilder.CreateIndex(
                name: "ix_invoice_report_formats_name",
                table: "InvoiceReportFormats",
                column: "name");

            migrationBuilder.CreateIndex(
                name: "ix_invoices_client_id",
                table: "Invoices",
                column: "client_id");

            migrationBuilder.CreateIndex(
                name: "ix_invoices_invoice_number",
                table: "Invoices",
                column: "invoice_number",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_invoices_quotation_id",
                table: "Invoices",
                column: "quotation_id",
                unique: true,
                filter: "[quotation_id] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "ix_invoices_report_format_id",
                table: "Invoices",
                column: "report_format_id");

            migrationBuilder.CreateIndex(
                name: "ix_invoice_trip_links_invoice_id",
                table: "InvoiceTripLinks",
                column: "invoice_id");

            migrationBuilder.CreateIndex(
                name: "ix_invoice_trip_links_invoice_id_trip_id",
                table: "InvoiceTripLinks",
                columns: new[] { "invoice_id", "trip_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_invoice_trip_links_trip_id",
                table: "InvoiceTripLinks",
                column: "trip_id");

            migrationBuilder.CreateIndex(
                name: "ix_outstanding_invoice_reports_client_id",
                table: "OutstandingInvoiceReports",
                column: "client_id");

            migrationBuilder.CreateIndex(
                name: "ix_outstanding_invoice_reports_period_year_period_month",
                table: "OutstandingInvoiceReports",
                columns: new[] { "period_year", "period_month" });

            migrationBuilder.CreateIndex(
                name: "ix_outstanding_invoice_snapshots_invoice_id",
                table: "OutstandingInvoiceSnapshots",
                column: "invoice_id");

            migrationBuilder.CreateIndex(
                name: "ix_outstanding_invoice_snapshots_outstanding_invoice_report_id",
                table: "OutstandingInvoiceSnapshots",
                column: "outstanding_invoice_report_id");

            migrationBuilder.CreateIndex(
                name: "ix_quotation_line_items_quotation_id",
                table: "QuotationLineItems",
                column: "quotation_id");

            migrationBuilder.CreateIndex(
                name: "ix_quotations_client_id",
                table: "Quotations",
                column: "client_id");

            migrationBuilder.CreateIndex(
                name: "ix_quotations_quotation_number",
                table: "Quotations",
                column: "quotation_number",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_report_format_columns_invoice_report_format_id",
                table: "ReportFormatColumns",
                column: "invoice_report_format_id");

            migrationBuilder.CreateIndex(
                name: "ix_report_format_columns_invoice_report_format_id_column_key",
                table: "ReportFormatColumns",
                columns: new[] { "invoice_report_format_id", "column_key" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClientPortalUsers");

            migrationBuilder.DropTable(
                name: "InvoiceLineItems");

            migrationBuilder.DropTable(
                name: "InvoicePayments");

            migrationBuilder.DropTable(
                name: "InvoiceReminderLogs");

            migrationBuilder.DropTable(
                name: "InvoiceTripLinks");

            migrationBuilder.DropTable(
                name: "OutstandingInvoiceSnapshots");

            migrationBuilder.DropTable(
                name: "QuotationLineItems");

            migrationBuilder.DropTable(
                name: "ReportFormatColumns");

            migrationBuilder.DropTable(
                name: "Invoices");

            migrationBuilder.DropTable(
                name: "OutstandingInvoiceReports");

            migrationBuilder.DropTable(
                name: "InvoiceReportFormats");

            migrationBuilder.DropTable(
                name: "Quotations");

            migrationBuilder.DropTable(
                name: "Clients");
        }
    }
}
