using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Database.Migrations;

/// <inheritdoc />
public partial class RemoveStopsHaltsVoucherPod : Migration
{
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "trip_custom_fields",
                schema: "public");

            migrationBuilder.DropTable(
                name: "trip_halts",
                schema: "public");

            migrationBuilder.DropTable(
                name: "trip_pod_uploads",
                schema: "public");

            migrationBuilder.DropTable(
                name: "custom_field_definitions",
                schema: "public");

            migrationBuilder.DropTable(
                name: "trip_vouchers",
                schema: "public");

            migrationBuilder.DropTable(
                name: "trip_stops",
                schema: "public");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "custom_field_definitions",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    applies_to = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    data_type = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    default_value = table.Column<string>(type: "text", nullable: true),
                    field_label = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    field_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    field_type = table.Column<string>(type: "text", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    is_required = table.Column<bool>(type: "boolean", nullable: false),
                    sort_order = table.Column<int>(type: "integer", nullable: false),
                    validation_regex = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_custom_field_definitions", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "trip_halts",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    trip_id = table.Column<Guid>(type: "uuid", nullable: false),
                    duration_minutes = table.Column<int>(type: "integer", nullable: true),
                    ended_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    halt_type = table.Column<int>(type: "integer", nullable: false),
                    latitude = table.Column<double>(type: "double precision", nullable: true),
                    location_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    longitude = table.Column<double>(type: "double precision", nullable: true),
                    reason = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    recorded_by_driver_id = table.Column<Guid>(type: "uuid", nullable: false),
                    started_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_trip_halts", x => x.id);
                    table.ForeignKey(
                        name: "fk_trip_halts_trips_trip_id",
                        column: x => x.trip_id,
                        principalSchema: "public",
                        principalTable: "trips",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "trip_stops",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    trip_id = table.Column<Guid>(type: "uuid", nullable: false),
                    actual_arrival_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    actual_departure_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    address = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    latitude = table.Column<double>(type: "double precision", nullable: true),
                    location_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    longitude = table.Column<double>(type: "double precision", nullable: true),
                    notes = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    poc_email = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    poc_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    poc_phone = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    scheduled_arrival_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    stop_order = table.Column<int>(type: "integer", nullable: false),
                    stop_type = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_trip_stops", x => x.id);
                    table.ForeignKey(
                        name: "fk_trip_stops_trips_trip_id",
                        column: x => x.trip_id,
                        principalSchema: "public",
                        principalTable: "trips",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "trip_vouchers",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    trip_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_by_user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    notes = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    voucher_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    voucher_number = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_trip_vouchers", x => x.id);
                    table.ForeignKey(
                        name: "fk_trip_vouchers_trips_trip_id",
                        column: x => x.trip_id,
                        principalSchema: "public",
                        principalTable: "trips",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "trip_pod_uploads",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    trip_id = table.Column<Guid>(type: "uuid", nullable: false),
                    trip_stop_id = table.Column<Guid>(type: "uuid", nullable: true),
                    document_type = table.Column<int>(type: "integer", nullable: false),
                    file_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    file_url = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    latitude = table.Column<double>(type: "double precision", nullable: true),
                    longitude = table.Column<double>(type: "double precision", nullable: true),
                    uploaded_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    uploaded_by_driver_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_trip_pod_uploads", x => x.id);
                    table.ForeignKey(
                        name: "fk_trip_pod_uploads_trip_stops_trip_stop_id",
                        column: x => x.trip_stop_id,
                        principalSchema: "public",
                        principalTable: "trip_stops",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_trip_pod_uploads_trips_trip_id",
                        column: x => x.trip_id,
                        principalSchema: "public",
                        principalTable: "trips",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "trip_custom_fields",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    field_definition_id = table.Column<Guid>(type: "uuid", nullable: false),
                    trip_id = table.Column<Guid>(type: "uuid", nullable: false),
                    trip_voucher_id = table.Column<Guid>(type: "uuid", nullable: true),
                    value = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_trip_custom_fields", x => x.id);
                    table.ForeignKey(
                        name: "fk_trip_custom_fields_custom_field_definitions_field_definitio",
                        column: x => x.field_definition_id,
                        principalSchema: "public",
                        principalTable: "custom_field_definitions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_trip_custom_fields_trip_vouchers_trip_voucher_id",
                        column: x => x.trip_voucher_id,
                        principalSchema: "public",
                        principalTable: "trip_vouchers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_trip_custom_fields_trips_trip_id",
                        column: x => x.trip_id,
                        principalSchema: "public",
                        principalTable: "trips",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_trip_custom_fields_field_definition_id",
                schema: "public",
                table: "trip_custom_fields",
                column: "field_definition_id");

            migrationBuilder.CreateIndex(
                name: "ix_trip_custom_fields_trip_id",
                schema: "public",
                table: "trip_custom_fields",
                column: "trip_id");

            migrationBuilder.CreateIndex(
                name: "ix_trip_custom_fields_trip_voucher_id",
                schema: "public",
                table: "trip_custom_fields",
                column: "trip_voucher_id");

            migrationBuilder.CreateIndex(
                name: "ix_trip_halts_trip_id",
                schema: "public",
                table: "trip_halts",
                column: "trip_id");

            migrationBuilder.CreateIndex(
                name: "ix_trip_pod_uploads_trip_id",
                schema: "public",
                table: "trip_pod_uploads",
                column: "trip_id");

            migrationBuilder.CreateIndex(
                name: "ix_trip_pod_uploads_trip_stop_id",
                schema: "public",
                table: "trip_pod_uploads",
                column: "trip_stop_id");

            migrationBuilder.CreateIndex(
                name: "ix_trip_stops_trip_id",
                schema: "public",
                table: "trip_stops",
                column: "trip_id");

            migrationBuilder.CreateIndex(
                name: "ix_trip_vouchers_trip_id",
                schema: "public",
                table: "trip_vouchers",
                column: "trip_id",
                unique: true);
        }
}
