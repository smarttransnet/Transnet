using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Database.Migrations;

/// <inheritdoc />
public partial class AddDriverModule : Migration
{
    private static readonly string[] _attendanceDateColumns = ["driver_id", "attendance_date"];
    private static readonly string[] _recordedAtColumns = ["driver_id", "recorded_at"];

    /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "drivers",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    employee_number = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    first_name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    last_name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    phone_number = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    licence_number = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    licence_expiry_date = table.Column<DateOnly>(type: "date", nullable: false),
                    nationality_code = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    sponsor_name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    status = table.Column<int>(type: "int", nullable: false),
                    is_active = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_drivers", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "driver_attendance_logs",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    driver_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    attendance_date = table.Column<DateOnly>(type: "date", nullable: false),
                    check_in_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    check_out_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    check_in_latitude = table.Column<double>(type: "float", nullable: true),
                    check_in_longitude = table.Column<double>(type: "float", nullable: true),
                    check_out_latitude = table.Column<double>(type: "float", nullable: true),
                    check_out_longitude = table.Column<double>(type: "float", nullable: true),
                    total_hours_worked = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: true),
                    notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    source = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_driver_attendance_logs", x => x.id);
                    table.ForeignKey(
                        name: "fk_driver_attendance_logs_drivers_driver_id",
                        column: x => x.driver_id,
                        principalTable: "drivers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "driver_auth_credentials",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    driver_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    username_hash = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    password_hash = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    refresh_token = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    refresh_token_expires_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    last_login_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    device_token = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    platform = table.Column<int>(type: "int", nullable: false),
                    is_locked = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    failed_attempts = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_driver_auth_credentials", x => x.id);
                    table.ForeignKey(
                        name: "fk_driver_auth_credentials_drivers_driver_id",
                        column: x => x.driver_id,
                        principalTable: "drivers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "driver_documents",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    driver_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    trip_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    document_type = table.Column<int>(type: "int", nullable: false),
                    title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    file_url = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: false),
                    uploaded_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    submitted_from_app = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    latitude = table.Column<double>(type: "float", nullable: true),
                    longitude = table.Column<double>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_driver_documents", x => x.id);
                    table.ForeignKey(
                        name: "fk_driver_documents_drivers_driver_id",
                        column: x => x.driver_id,
                        principalTable: "drivers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "driver_expenses",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    driver_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    trip_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    expense_type = table.Column<int>(type: "int", nullable: false),
                    amount_qar = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    expense_date = table.Column<DateOnly>(type: "date", nullable: false),
                    description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    receipt_url = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: true),
                    fuel_litres = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: true),
                    fuel_station = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    odometer_reading = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    status = table.Column<int>(type: "int", nullable: false),
                    reviewed_by_user_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    reviewed_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    submitted_at = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_driver_expenses", x => x.id);
                    table.ForeignKey(
                        name: "fk_driver_expenses_drivers_driver_id",
                        column: x => x.driver_id,
                        principalTable: "drivers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "driver_gps_logs",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    driver_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    trip_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    session_start = table.Column<DateTime>(type: "datetime2", nullable: false),
                    session_end = table.Column<DateTime>(type: "datetime2", nullable: true),
                    total_distance_km = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    max_speed_kmh = table.Column<float>(type: "real", nullable: true),
                    point_count = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    raw_track_url = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_driver_gps_logs", x => x.id);
                    table.ForeignKey(
                        name: "fk_driver_gps_logs_drivers_driver_id",
                        column: x => x.driver_id,
                        principalTable: "drivers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "driver_notifications",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    driver_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    notification_type = table.Column<int>(type: "int", nullable: false),
                    title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    body = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    related_entity_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    related_entity_type = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    is_read = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    read_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    sent_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    channel = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_driver_notifications", x => x.id);
                    table.ForeignKey(
                        name: "fk_driver_notifications_drivers_driver_id",
                        column: x => x.driver_id,
                        principalTable: "drivers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "driver_trip_assignments",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    driver_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    trip_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    assigned_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    assigned_by_user_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    accepted_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    rejected_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    rejection_reason = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    assignment_status = table.Column<int>(type: "int", nullable: false),
                    displayed_in_app_at = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_driver_trip_assignments", x => x.id);
                    table.ForeignKey(
                        name: "fk_driver_trip_assignments_drivers_driver_id",
                        column: x => x.driver_id,
                        principalTable: "drivers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "driver_location_updates",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    driver_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    trip_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    latitude = table.Column<double>(type: "float", nullable: false),
                    longitude = table.Column<double>(type: "float", nullable: false),
                    accuracy = table.Column<float>(type: "real", nullable: true),
                    speed_kmh = table.Column<float>(type: "real", nullable: true),
                    bearing = table.Column<float>(type: "real", nullable: true),
                    recorded_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    source = table.Column<int>(type: "int", nullable: false),
                    driver_gps_log_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_driver_location_updates", x => x.id);
                    table.ForeignKey(
                        name: "fk_driver_location_updates_driver_gps_logs_driver_gps_log_id",
                        column: x => x.driver_gps_log_id,
                        principalTable: "driver_gps_logs",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_driver_location_updates_drivers_driver_id",
                        column: x => x.driver_id,
                        principalTable: "drivers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_driver_attendance_logs_driver_id",
                table: "driver_attendance_logs",
                column: "driver_id");

            migrationBuilder.CreateIndex(
                name: "ix_driver_attendance_logs_driver_id_attendance_date",
                table: "driver_attendance_logs",
                columns: _attendanceDateColumns);

            migrationBuilder.CreateIndex(
                name: "ix_driver_auth_credentials_driver_id",
                table: "driver_auth_credentials",
                column: "driver_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_driver_documents_driver_id",
                table: "driver_documents",
                column: "driver_id");

            migrationBuilder.CreateIndex(
                name: "ix_driver_expenses_driver_id",
                table: "driver_expenses",
                column: "driver_id");

            migrationBuilder.CreateIndex(
                name: "ix_driver_expenses_trip_id",
                table: "driver_expenses",
                column: "trip_id");

            migrationBuilder.CreateIndex(
                name: "ix_driver_gps_logs_driver_id",
                table: "driver_gps_logs",
                column: "driver_id");

            migrationBuilder.CreateIndex(
                name: "ix_driver_location_updates_driver_gps_log_id",
                table: "driver_location_updates",
                column: "driver_gps_log_id");

            migrationBuilder.CreateIndex(
                name: "ix_driver_location_updates_driver_id",
                table: "driver_location_updates",
                column: "driver_id");

            migrationBuilder.CreateIndex(
                name: "ix_driver_location_updates_driver_id_recorded_at",
                table: "driver_location_updates",
                columns: _recordedAtColumns);

            migrationBuilder.CreateIndex(
                name: "ix_driver_notifications_driver_id",
                table: "driver_notifications",
                column: "driver_id");

            migrationBuilder.CreateIndex(
                name: "ix_driver_trip_assignments_driver_id",
                table: "driver_trip_assignments",
                column: "driver_id");

            migrationBuilder.CreateIndex(
                name: "ix_driver_trip_assignments_trip_id",
                table: "driver_trip_assignments",
                column: "trip_id");

            migrationBuilder.CreateIndex(
                name: "ix_drivers_email",
                table: "drivers",
                column: "email",
                unique: true,
                filter: "[email] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "ix_drivers_employee_number",
                table: "drivers",
                column: "employee_number",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_drivers_phone_number",
                table: "drivers",
                column: "phone_number",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "driver_attendance_logs");

            migrationBuilder.DropTable(
                name: "driver_auth_credentials");

            migrationBuilder.DropTable(
                name: "driver_documents");

            migrationBuilder.DropTable(
                name: "driver_expenses");

            migrationBuilder.DropTable(
                name: "driver_location_updates");

            migrationBuilder.DropTable(
                name: "driver_notifications");

            migrationBuilder.DropTable(
                name: "driver_trip_assignments");

            migrationBuilder.DropTable(
                name: "driver_gps_logs");

            migrationBuilder.DropTable(
                name: "drivers");
        }
    }
