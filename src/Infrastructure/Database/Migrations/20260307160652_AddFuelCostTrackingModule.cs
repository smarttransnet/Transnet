#pragma warning disable IDE0161, CA1861
using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddFuelCostTrackingModule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "driver_salary_records",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    driver_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    period_month = table.Column<int>(type: "int", nullable: false),
                    period_year = table.Column<int>(type: "int", nullable: false),
                    base_salary_qar = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    allowances_qar = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    overtime_qar = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    deductions_qar = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    commission_qar = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    net_payable_qar = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    sponsor_approval_status = table.Column<int>(type: "int", nullable: false),
                    sponsor_approved_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    sponsor_approved_by_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    notes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_driver_salary_records", x => x.id);
                    table.ForeignKey(
                        name: "fk_driver_salary_records_drivers_driver_id",
                        column: x => x.driver_id,
                        principalTable: "drivers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "monthly_expense_reports",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    period_month = table.Column<int>(type: "int", nullable: false),
                    period_year = table.Column<int>(type: "int", nullable: false),
                    report_type = table.Column<int>(type: "int", nullable: false),
                    generated_by_user_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    generated_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    total_fuel_cost_qar = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    total_salary_cost_qar = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    total_driver_expenses_qar = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    total_operational_cost_qar = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    status = table.Column<int>(type: "int", nullable: false),
                    exported_file_url = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    notes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_monthly_expense_reports", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "vehicle_fuel_summaries",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    vehicle_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    period_month = table.Column<int>(type: "int", nullable: false),
                    period_year = table.Column<int>(type: "int", nullable: false),
                    total_litres = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    total_cost_qar = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    average_cost_per_litre_qar = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    average_fuel_efficiency_km_per_l = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    woqood_transaction_count = table.Column<int>(type: "int", nullable: false),
                    driver_entry_count = table.Column<int>(type: "int", nullable: false),
                    last_updated_at = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_vehicle_fuel_summaries", x => x.id);
                    table.ForeignKey(
                        name: "fk_vehicle_fuel_summaries_vehicles_vehicle_id",
                        column: x => x.vehicle_id,
                        principalTable: "vehicles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "woqood_card_mappings",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    woqood_card_number = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    vehicle_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    driver_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    card_holder_name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    is_active = table.Column<bool>(type: "bit", nullable: false),
                    mapped_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    mapped_by_user_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_woqood_card_mappings", x => x.id);
                    table.ForeignKey(
                        name: "fk_woqood_card_mappings_drivers_driver_id",
                        column: x => x.driver_id,
                        principalTable: "drivers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "fk_woqood_card_mappings_vehicles_vehicle_id",
                        column: x => x.vehicle_id,
                        principalTable: "vehicles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "woqood_import_batches",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    file_name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    uploaded_by_user_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    uploaded_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    period_month = table.Column<int>(type: "int", nullable: false),
                    period_year = table.Column<int>(type: "int", nullable: false),
                    total_rows = table.Column<int>(type: "int", nullable: false),
                    success_count = table.Column<int>(type: "int", nullable: false),
                    failure_count = table.Column<int>(type: "int", nullable: false),
                    total_litres = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    total_amount_qar = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    status = table.Column<int>(type: "int", nullable: false),
                    error_summary = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_woqood_import_batches", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "driver_commission_items",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    driver_salary_record_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    trip_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    commission_type = table.Column<int>(type: "int", nullable: false),
                    description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    amount_qar = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    calculation_basis = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    applied_at = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_driver_commission_items", x => x.id);
                    table.ForeignKey(
                        name: "fk_driver_commission_items_driver_salary_records_driver_salary_record_id",
                        column: x => x.driver_salary_record_id,
                        principalTable: "driver_salary_records",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_driver_commission_items_trips_trip_id",
                        column: x => x.trip_id,
                        principalTable: "trips",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "salary_expense_lines",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    driver_salary_record_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    driver_expense_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    line_type = table.Column<int>(type: "int", nullable: false),
                    description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    amount_qar = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    is_deduction = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_salary_expense_lines", x => x.id);
                    table.ForeignKey(
                        name: "fk_salary_expense_lines_driver_expenses_driver_expense_id",
                        column: x => x.driver_expense_id,
                        principalTable: "driver_expenses",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "fk_salary_expense_lines_driver_salary_records_driver_salary_record_id",
                        column: x => x.driver_salary_record_id,
                        principalTable: "driver_salary_records",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "expense_report_line_items",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    monthly_expense_report_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    category = table.Column<int>(type: "int", nullable: false),
                    sub_category = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    entity_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    entity_type = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    amount_qar = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    sort_order = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_expense_report_line_items", x => x.id);
                    table.ForeignKey(
                        name: "fk_expense_report_line_items_monthly_expense_reports_monthly_expense_report_id",
                        column: x => x.monthly_expense_report_id,
                        principalTable: "monthly_expense_reports",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "woqood_fuel_transactions",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    woqood_import_batch_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    woqood_card_number = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    vehicle_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    driver_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    trip_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    transaction_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    station_name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    fuel_type = table.Column<int>(type: "int", nullable: false),
                    quantity_litres = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    unit_price_qar = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    total_amount_qar = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    odometer = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    is_allocated = table.Column<bool>(type: "bit", nullable: false),
                    raw_row_data = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_woqood_fuel_transactions", x => x.id);
                    table.ForeignKey(
                        name: "fk_woqood_fuel_transactions_woqood_import_batches_woqood_import_batch_id",
                        column: x => x.woqood_import_batch_id,
                        principalTable: "woqood_import_batches",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "fuel_cost_allocations",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    woqood_fuel_transaction_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    driver_expense_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    vehicle_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    trip_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    allocation_source = table.Column<int>(type: "int", nullable: false),
                    quantity_litres = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    amount_qar = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    allocation_date = table.Column<DateOnly>(type: "date", nullable: false),
                    allocated_by_user_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_fuel_cost_allocations", x => x.id);
                    table.ForeignKey(
                        name: "fk_fuel_cost_allocations_driver_expenses_driver_expense_id",
                        column: x => x.driver_expense_id,
                        principalTable: "driver_expenses",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "fk_fuel_cost_allocations_trips_trip_id",
                        column: x => x.trip_id,
                        principalTable: "trips",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "fk_fuel_cost_allocations_vehicles_vehicle_id",
                        column: x => x.vehicle_id,
                        principalTable: "vehicles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_fuel_cost_allocations_woqood_fuel_transactions_woqood_fuel_transaction_id",
                        column: x => x.woqood_fuel_transaction_id,
                        principalTable: "woqood_fuel_transactions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "ix_driver_commission_items_driver_salary_record_id",
                table: "driver_commission_items",
                column: "driver_salary_record_id");

            migrationBuilder.CreateIndex(
                name: "ix_driver_commission_items_trip_id",
                table: "driver_commission_items",
                column: "trip_id");

            migrationBuilder.CreateIndex(
                name: "ix_driver_salary_records_driver_id_period_year_period_month",
                table: "driver_salary_records",
                columns: new[] { "driver_id", "period_year", "period_month" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_expense_report_line_items_monthly_expense_report_id",
                table: "expense_report_line_items",
                column: "monthly_expense_report_id");

            migrationBuilder.CreateIndex(
                name: "ix_fuel_cost_allocations_driver_expense_id",
                table: "fuel_cost_allocations",
                column: "driver_expense_id");

            migrationBuilder.CreateIndex(
                name: "ix_fuel_cost_allocations_trip_id",
                table: "fuel_cost_allocations",
                column: "trip_id");

            migrationBuilder.CreateIndex(
                name: "ix_fuel_cost_allocations_vehicle_id",
                table: "fuel_cost_allocations",
                column: "vehicle_id");

            migrationBuilder.CreateIndex(
                name: "ix_fuel_cost_allocations_woqood_fuel_transaction_id",
                table: "fuel_cost_allocations",
                column: "woqood_fuel_transaction_id",
                unique: true,
                filter: "[woqood_fuel_transaction_id] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "ix_monthly_expense_reports_period_year_period_month_report_type",
                table: "monthly_expense_reports",
                columns: new[] { "period_year", "period_month", "report_type" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_salary_expense_lines_driver_expense_id",
                table: "salary_expense_lines",
                column: "driver_expense_id");

            migrationBuilder.CreateIndex(
                name: "ix_salary_expense_lines_driver_salary_record_id",
                table: "salary_expense_lines",
                column: "driver_salary_record_id");

            migrationBuilder.CreateIndex(
                name: "ix_vehicle_fuel_summaries_vehicle_id_period_year_period_month",
                table: "vehicle_fuel_summaries",
                columns: new[] { "vehicle_id", "period_year", "period_month" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_woqood_card_mappings_driver_id",
                table: "woqood_card_mappings",
                column: "driver_id");

            migrationBuilder.CreateIndex(
                name: "ix_woqood_card_mappings_vehicle_id",
                table: "woqood_card_mappings",
                column: "vehicle_id");

            migrationBuilder.CreateIndex(
                name: "ix_woqood_card_mappings_woqood_card_number",
                table: "woqood_card_mappings",
                column: "woqood_card_number",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_woqood_fuel_transactions_woqood_import_batch_id",
                table: "woqood_fuel_transactions",
                column: "woqood_import_batch_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "driver_commission_items");

            migrationBuilder.DropTable(
                name: "expense_report_line_items");

            migrationBuilder.DropTable(
                name: "fuel_cost_allocations");

            migrationBuilder.DropTable(
                name: "salary_expense_lines");

            migrationBuilder.DropTable(
                name: "vehicle_fuel_summaries");

            migrationBuilder.DropTable(
                name: "woqood_card_mappings");

            migrationBuilder.DropTable(
                name: "monthly_expense_reports");

            migrationBuilder.DropTable(
                name: "woqood_fuel_transactions");

            migrationBuilder.DropTable(
                name: "driver_salary_records");

            migrationBuilder.DropTable(
                name: "woqood_import_batches");
        }
    }
}
