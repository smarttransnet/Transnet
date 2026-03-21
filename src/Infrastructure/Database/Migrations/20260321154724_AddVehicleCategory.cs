using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Database.Migrations;

/// <inheritdoc />
public partial class AddVehicleCategory : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "asset_locations",
            schema: "public",
            columns: table => new
            {
                id = table.Column<Guid>(type: "uuid", nullable: false),
                asset_id = table.Column<Guid>(type: "uuid", nullable: false),
                asset_type = table.Column<int>(type: "integer", nullable: false),
                latitude = table.Column<double>(type: "double precision", nullable: false),
                longitude = table.Column<double>(type: "double precision", nullable: false),
                location_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                is_assigned = table.Column<bool>(type: "boolean", nullable: false),
                recorded_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                source = table.Column<int>(type: "integer", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_asset_locations", x => x.id);
            });

        migrationBuilder.CreateTable(
            name: "vehicle_categories",
            schema: "public",
            columns: table => new
            {
                id = table.Column<Guid>(type: "uuid", nullable: false),
                name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                is_active = table.Column<bool>(type: "boolean", nullable: false),
                created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_vehicle_categories", x => x.id);
            });

        migrationBuilder.CreateTable(
            name: "vehicles",
            schema: "public",
            columns: table => new
            {
                id = table.Column<Guid>(type: "uuid", nullable: false),
                registration_number = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                plate_number = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                make = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                model = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                year = table.Column<int>(type: "integer", nullable: false),
                vehicle_category_id = table.Column<Guid>(type: "uuid", nullable: false),
                vehicle_type = table.Column<int>(type: "integer", nullable: false),
                status = table.Column<int>(type: "integer", nullable: false),
                current_driver_id = table.Column<Guid>(type: "uuid", nullable: true),
                current_location_id = table.Column<Guid>(type: "uuid", nullable: true),
                odometer_reading = table.Column<decimal>(type: "numeric", nullable: false),
                created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                is_active = table.Column<bool>(type: "boolean", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_vehicles", x => x.id);
                table.ForeignKey(
                    name: "fk_vehicles_vehicle_categories_vehicle_category_id",
                    column: x => x.vehicle_category_id,
                    principalSchema: "public",
                    principalTable: "vehicle_categories",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Restrict);
            });

        migrationBuilder.CreateTable(
            name: "trailers",
            schema: "public",
            columns: table => new
            {
                id = table.Column<Guid>(type: "uuid", nullable: false),
                trailer_number = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                trailer_type = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                capacity = table.Column<decimal>(type: "numeric", nullable: false),
                capacity_unit = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                attached_vehicle_id = table.Column<Guid>(type: "uuid", nullable: true),
                status = table.Column<int>(type: "integer", nullable: false),
                current_location_id = table.Column<Guid>(type: "uuid", nullable: true),
                total_revenue_qar = table.Column<decimal>(type: "numeric", nullable: false),
                total_expenses_qar = table.Column<decimal>(type: "numeric", nullable: false),
                is_active = table.Column<bool>(type: "boolean", nullable: false),
                created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_trailers", x => x.id);
                table.ForeignKey(
                    name: "fk_trailers_vehicles_attached_vehicle_id",
                    column: x => x.attached_vehicle_id,
                    principalSchema: "public",
                    principalTable: "vehicles",
                    principalColumn: "id",
                    onDelete: ReferentialAction.SetNull);
            });

        migrationBuilder.CreateIndex(
            name: "ix_trailers_attached_vehicle_id",
            schema: "public",
            table: "trailers",
            column: "attached_vehicle_id");

        migrationBuilder.CreateIndex(
            name: "ix_vehicles_vehicle_category_id",
            schema: "public",
            table: "vehicles",
            column: "vehicle_category_id");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "asset_locations",
            schema: "public");

        migrationBuilder.DropTable(
            name: "trailers",
            schema: "public");

        migrationBuilder.DropTable(
            name: "vehicles",
            schema: "public");

        migrationBuilder.DropTable(
            name: "vehicle_categories",
            schema: "public");
    }
}
