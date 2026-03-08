using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Database.Migrations;
    /// <inheritdoc />
    public partial class AddTripModule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "asset_locations",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    asset_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    asset_type = table.Column<int>(type: "int", nullable: false),
                    latitude = table.Column<double>(type: "float", nullable: false),
                    longitude = table.Column<double>(type: "float", nullable: false),
                    location_name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    is_assigned = table.Column<bool>(type: "bit", nullable: false),
                    recorded_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    source = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_asset_locations", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "custom_field_definitions",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    field_name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    field_label = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    data_type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    applies_to = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    is_required = table.Column<bool>(type: "bit", nullable: false),
                    is_active = table.Column<bool>(type: "bit", nullable: false),
                    sort_order = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_custom_field_definitions", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "import_batches",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    file_name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    uploaded_by_user_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    uploaded_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    total_rows = table.Column<int>(type: "int", nullable: false),
                    success_count = table.Column<int>(type: "int", nullable: false),
                    failure_count = table.Column<int>(type: "int", nullable: false),
                    status = table.Column<int>(type: "int", nullable: false),
                    error_summary = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_import_batches", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "inspection_checklists",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    inspection_type = table.Column<int>(type: "int", nullable: false),
                    applicable_vehicle_types = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    is_active = table.Column<bool>(type: "bit", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_inspection_checklists", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    email = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    first_name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    last_name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    password_hash = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_users", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "vehicle_categories",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    is_active = table.Column<bool>(type: "bit", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_vehicle_categories", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "trips",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    trip_number = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    driver_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    vehicle_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    trailer_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    status = table.Column<int>(type: "int", nullable: false),
                    scheduled_start_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    actual_start_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    actual_end_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    total_distance_km = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    is_imported = table.Column<bool>(type: "bit", nullable: false),
                    import_batch_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    driver_confirmed_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    office_approved_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    office_approved_by_user_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_trips", x => x.id);
                    table.ForeignKey(
                        name: "fk_trips_import_batches_import_batch_id",
                        column: x => x.import_batch_id,
                        principalTable: "import_batches",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "checklist_items",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    inspection_checklist_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    item_name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    category = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    is_required = table.Column<bool>(type: "bit", nullable: false),
                    sort_order = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_checklist_items", x => x.id);
                    table.ForeignKey(
                        name: "fk_checklist_items_inspection_checklists_inspection_checklist_id",
                        column: x => x.inspection_checklist_id,
                        principalTable: "inspection_checklists",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "todo_items",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    user_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    due_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    labels = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    is_completed = table.Column<bool>(type: "bit", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    completed_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    priority = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_todo_items", x => x.id);
                    table.ForeignKey(
                        name: "fk_todo_items_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "vehicles",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    registration_number = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    plate_number = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    make = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    model = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    year = table.Column<int>(type: "int", nullable: false),
                    vehicle_category_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    vehicle_type = table.Column<int>(type: "int", nullable: false),
                    status = table.Column<int>(type: "int", nullable: false),
                    current_driver_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    current_location_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    odometer_reading = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    is_active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_vehicles", x => x.id);
                    table.ForeignKey(
                        name: "fk_vehicles_vehicle_categories_vehicle_category_id",
                        column: x => x.vehicle_category_id,
                        principalTable: "vehicle_categories",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "trip_halts",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    trip_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    halt_type = table.Column<int>(type: "int", nullable: false),
                    reason = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    latitude = table.Column<double>(type: "float", nullable: true),
                    longitude = table.Column<double>(type: "float", nullable: true),
                    location_name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    started_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ended_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    duration_minutes = table.Column<int>(type: "int", nullable: true),
                    recorded_by_driver_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_trip_halts", x => x.id);
                    table.ForeignKey(
                        name: "fk_trip_halts_trips_trip_id",
                        column: x => x.trip_id,
                        principalTable: "trips",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "trip_status_histories",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    trip_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    previous_status = table.Column<int>(type: "int", nullable: false),
                    new_status = table.Column<int>(type: "int", nullable: false),
                    changed_by_user_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    changed_by_driver_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    changed_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    notes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    source = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_trip_status_histories", x => x.id);
                    table.ForeignKey(
                        name: "fk_trip_status_histories_trips_trip_id",
                        column: x => x.trip_id,
                        principalTable: "trips",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "trip_stops",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    trip_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    stop_order = table.Column<int>(type: "int", nullable: false),
                    stop_type = table.Column<int>(type: "int", nullable: false),
                    location_name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    address = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    latitude = table.Column<double>(type: "float", nullable: true),
                    longitude = table.Column<double>(type: "float", nullable: true),
                    poc_name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    poc_phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    poc_email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    scheduled_arrival_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    actual_arrival_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    actual_departure_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    notes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_trip_stops", x => x.id);
                    table.ForeignKey(
                        name: "fk_trip_stops_trips_trip_id",
                        column: x => x.trip_id,
                        principalTable: "trips",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "trip_vouchers",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    trip_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    voucher_number = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    voucher_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    notes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    created_by_user_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_trip_vouchers", x => x.id);
                    table.ForeignKey(
                        name: "fk_trip_vouchers_trips_trip_id",
                        column: x => x.trip_id,
                        principalTable: "trips",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "trailers",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    trailer_number = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    trailer_type = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    capacity = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    capacity_unit = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    attached_vehicle_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    status = table.Column<int>(type: "int", nullable: false),
                    current_location_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    total_revenue_qar = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    total_expenses_qar = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    is_active = table.Column<bool>(type: "bit", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_trailers", x => x.id);
                    table.ForeignKey(
                        name: "fk_trailers_vehicles_attached_vehicle_id",
                        column: x => x.attached_vehicle_id,
                        principalTable: "vehicles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "vehicle_inspections",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    vehicle_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    inspection_checklist_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    inspection_type = table.Column<int>(type: "int", nullable: false),
                    driver_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    trip_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    inspected_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    driver_signature = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    driver_signed_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    notes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    odometer_reading = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_vehicle_inspections", x => x.id);
                    table.ForeignKey(
                        name: "fk_vehicle_inspections_inspection_checklists_inspection_checklist_id",
                        column: x => x.inspection_checklist_id,
                        principalTable: "inspection_checklists",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_vehicle_inspections_vehicles_vehicle_id",
                        column: x => x.vehicle_id,
                        principalTable: "vehicles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "trip_pod_uploads",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    trip_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    trip_stop_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    document_type = table.Column<int>(type: "int", nullable: false),
                    file_url = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    file_name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    latitude = table.Column<double>(type: "float", nullable: true),
                    longitude = table.Column<double>(type: "float", nullable: true),
                    uploaded_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    uploaded_by_driver_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_trip_pod_uploads", x => x.id);
                    table.ForeignKey(
                        name: "fk_trip_pod_uploads_trip_stops_trip_stop_id",
                        column: x => x.trip_stop_id,
                        principalTable: "trip_stops",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_trip_pod_uploads_trips_trip_id",
                        column: x => x.trip_id,
                        principalTable: "trips",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "trip_custom_fields",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    trip_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    trip_voucher_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    field_definition_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    value = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_trip_custom_fields", x => x.id);
                    table.ForeignKey(
                        name: "fk_trip_custom_fields_custom_field_definitions_field_definition_id",
                        column: x => x.field_definition_id,
                        principalTable: "custom_field_definitions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_trip_custom_fields_trip_vouchers_trip_voucher_id",
                        column: x => x.trip_voucher_id,
                        principalTable: "trip_vouchers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_trip_custom_fields_trips_trip_id",
                        column: x => x.trip_id,
                        principalTable: "trips",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "inspection_photos",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    vehicle_inspection_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    photo_url = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    caption = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    photo_type = table.Column<int>(type: "int", nullable: false),
                    uploaded_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    uploaded_by_driver_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_inspection_photos", x => x.id);
                    table.ForeignKey(
                        name: "fk_inspection_photos_vehicle_inspections_vehicle_inspection_id",
                        column: x => x.vehicle_inspection_id,
                        principalTable: "vehicle_inspections",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "inspection_results",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    vehicle_inspection_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    checklist_item_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    is_passed = table.Column<bool>(type: "bit", nullable: false),
                    remarks = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    recorded_at = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_inspection_results", x => x.id);
                    table.ForeignKey(
                        name: "fk_inspection_results_checklist_items_checklist_item_id",
                        column: x => x.checklist_item_id,
                        principalTable: "checklist_items",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_inspection_results_vehicle_inspections_vehicle_inspection_id",
                        column: x => x.vehicle_inspection_id,
                        principalTable: "vehicle_inspections",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "work_orders",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    work_order_number = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    vehicle_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    vehicle_inspection_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    description = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    priority = table.Column<int>(type: "int", nullable: false),
                    status = table.Column<int>(type: "int", nullable: false),
                    assigned_technician_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    estimated_cost_qar = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    actual_cost_qar = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    scheduled_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    started_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    completed_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_work_orders", x => x.id);
                    table.ForeignKey(
                        name: "fk_work_orders_vehicle_inspections_vehicle_inspection_id",
                        column: x => x.vehicle_inspection_id,
                        principalTable: "vehicle_inspections",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "fk_work_orders_vehicles_vehicle_id",
                        column: x => x.vehicle_id,
                        principalTable: "vehicles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "work_order_items",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    work_order_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    item_type = table.Column<int>(type: "int", nullable: false),
                    description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    quantity = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    unit_cost_qar = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    total_cost_qar = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_work_order_items", x => x.id);
                    table.ForeignKey(
                        name: "fk_work_order_items_work_orders_work_order_id",
                        column: x => x.work_order_id,
                        principalTable: "work_orders",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "work_order_status_histories",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    work_order_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    previous_status = table.Column<int>(type: "int", nullable: false),
                    new_status = table.Column<int>(type: "int", nullable: false),
                    changed_by_user_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    changed_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    notes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_work_order_status_histories", x => x.id);
                    table.ForeignKey(
                        name: "fk_work_order_status_histories_work_orders_work_order_id",
                        column: x => x.work_order_id,
                        principalTable: "work_orders",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_checklist_items_inspection_checklist_id",
                table: "checklist_items",
                column: "inspection_checklist_id");

            migrationBuilder.CreateIndex(
                name: "ix_inspection_photos_vehicle_inspection_id",
                table: "inspection_photos",
                column: "vehicle_inspection_id");

            migrationBuilder.CreateIndex(
                name: "ix_inspection_results_checklist_item_id",
                table: "inspection_results",
                column: "checklist_item_id");

            migrationBuilder.CreateIndex(
                name: "ix_inspection_results_vehicle_inspection_id",
                table: "inspection_results",
                column: "vehicle_inspection_id");

            migrationBuilder.CreateIndex(
                name: "ix_todo_items_user_id",
                table: "todo_items",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_trailers_attached_vehicle_id",
                table: "trailers",
                column: "attached_vehicle_id");

            migrationBuilder.CreateIndex(
                name: "ix_trip_custom_fields_field_definition_id",
                table: "trip_custom_fields",
                column: "field_definition_id");

            migrationBuilder.CreateIndex(
                name: "ix_trip_custom_fields_trip_id",
                table: "trip_custom_fields",
                column: "trip_id");

            migrationBuilder.CreateIndex(
                name: "ix_trip_custom_fields_trip_voucher_id",
                table: "trip_custom_fields",
                column: "trip_voucher_id");

            migrationBuilder.CreateIndex(
                name: "ix_trip_halts_trip_id",
                table: "trip_halts",
                column: "trip_id");

            migrationBuilder.CreateIndex(
                name: "ix_trip_pod_uploads_trip_id",
                table: "trip_pod_uploads",
                column: "trip_id");

            migrationBuilder.CreateIndex(
                name: "ix_trip_pod_uploads_trip_stop_id",
                table: "trip_pod_uploads",
                column: "trip_stop_id");

            migrationBuilder.CreateIndex(
                name: "ix_trip_status_histories_trip_id",
                table: "trip_status_histories",
                column: "trip_id");

            migrationBuilder.CreateIndex(
                name: "ix_trip_stops_trip_id",
                table: "trip_stops",
                column: "trip_id");

            migrationBuilder.CreateIndex(
                name: "ix_trip_vouchers_trip_id",
                table: "trip_vouchers",
                column: "trip_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_trips_import_batch_id",
                table: "trips",
                column: "import_batch_id");

            migrationBuilder.CreateIndex(
                name: "ix_users_email",
                table: "users",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_vehicle_inspections_inspection_checklist_id",
                table: "vehicle_inspections",
                column: "inspection_checklist_id");

            migrationBuilder.CreateIndex(
                name: "ix_vehicle_inspections_vehicle_id",
                table: "vehicle_inspections",
                column: "vehicle_id");

            migrationBuilder.CreateIndex(
                name: "ix_vehicles_vehicle_category_id",
                table: "vehicles",
                column: "vehicle_category_id");

            migrationBuilder.CreateIndex(
                name: "ix_work_order_items_work_order_id",
                table: "work_order_items",
                column: "work_order_id");

            migrationBuilder.CreateIndex(
                name: "ix_work_order_status_histories_work_order_id",
                table: "work_order_status_histories",
                column: "work_order_id");

            migrationBuilder.CreateIndex(
                name: "ix_work_orders_vehicle_id",
                table: "work_orders",
                column: "vehicle_id");

            migrationBuilder.CreateIndex(
                name: "ix_work_orders_vehicle_inspection_id",
                table: "work_orders",
                column: "vehicle_inspection_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "asset_locations");

            migrationBuilder.DropTable(
                name: "inspection_photos");

            migrationBuilder.DropTable(
                name: "inspection_results");

            migrationBuilder.DropTable(
                name: "todo_items");

            migrationBuilder.DropTable(
                name: "trailers");

            migrationBuilder.DropTable(
                name: "trip_custom_fields");

            migrationBuilder.DropTable(
                name: "trip_halts");

            migrationBuilder.DropTable(
                name: "trip_pod_uploads");

            migrationBuilder.DropTable(
                name: "trip_status_histories");

            migrationBuilder.DropTable(
                name: "work_order_items");

            migrationBuilder.DropTable(
                name: "work_order_status_histories");

            migrationBuilder.DropTable(
                name: "checklist_items");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "custom_field_definitions");

            migrationBuilder.DropTable(
                name: "trip_vouchers");

            migrationBuilder.DropTable(
                name: "trip_stops");

            migrationBuilder.DropTable(
                name: "work_orders");

            migrationBuilder.DropTable(
                name: "trips");

            migrationBuilder.DropTable(
                name: "vehicle_inspections");

            migrationBuilder.DropTable(
                name: "import_batches");

            migrationBuilder.DropTable(
                name: "inspection_checklists");

            migrationBuilder.DropTable(
                name: "vehicles");

            migrationBuilder.DropTable(
                name: "vehicle_categories");
        }
    }

