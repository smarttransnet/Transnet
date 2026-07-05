using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class RemoveTripCategoriesAndMaterial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_trips_trip_category_materials_trip_category_material_id",
                schema: "public",
                table: "trips");

            migrationBuilder.DropTable(
                name: "materials",
                schema: "public");

            migrationBuilder.DropTable(
                name: "trip_category_materials",
                schema: "public");

            migrationBuilder.DropTable(
                name: "trip_categories",
                schema: "public");

            migrationBuilder.RenameColumn(
                name: "trip_category_material_id",
                schema: "public",
                table: "trips",
                newName: "vehicle_category_uom_id");

            migrationBuilder.RenameIndex(
                name: "ix_trips_trip_category_material_id",
                schema: "public",
                table: "trips",
                newName: "ix_trips_vehicle_category_uom_id");

            migrationBuilder.Sql("UPDATE public.trips SET vehicle_category_uom_id = NULL;");

            migrationBuilder.CreateTable(
                name: "vehicle_category_uoms",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    vehicle_category_id = table.Column<Guid>(type: "uuid", nullable: false),
                    uom_id = table.Column<Guid>(type: "uuid", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    created_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: false),
                    modified_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    modified_by = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_vehicle_category_uoms", x => x.id);
                    table.ForeignKey(
                        name: "fk_vehicle_category_uoms_uoms_uom_id",
                        column: x => x.uom_id,
                        principalSchema: "public",
                        principalTable: "uoms",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_vehicle_category_uoms_vehicle_categories_vehicle_category_id",
                        column: x => x.vehicle_category_id,
                        principalSchema: "public",
                        principalTable: "vehicle_categories",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_vehicle_category_uoms_uom_id",
                schema: "public",
                table: "vehicle_category_uoms",
                column: "uom_id");

            migrationBuilder.CreateIndex(
                name: "ix_vehicle_category_uoms_vehicle_category_id_uom_id",
                schema: "public",
                table: "vehicle_category_uoms",
                columns: new[] { "vehicle_category_id", "uom_id" },
                unique: true,
                filter: "is_active = true");

            migrationBuilder.AddForeignKey(
                name: "fk_trips_vehicle_category_uoms_vehicle_category_uom_id",
                schema: "public",
                table: "trips",
                column: "vehicle_category_uom_id",
                principalSchema: "public",
                principalTable: "vehicle_category_uoms",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_trips_vehicle_category_uoms_vehicle_category_uom_id",
                schema: "public",
                table: "trips");

            migrationBuilder.DropTable(
                name: "vehicle_category_uoms",
                schema: "public");

            migrationBuilder.RenameColumn(
                name: "vehicle_category_uom_id",
                schema: "public",
                table: "trips",
                newName: "trip_category_material_id");

            migrationBuilder.RenameIndex(
                name: "ix_trips_vehicle_category_uom_id",
                schema: "public",
                table: "trips",
                newName: "ix_trips_trip_category_material_id");

            migrationBuilder.Sql("UPDATE public.trips SET trip_category_material_id = NULL;");

            migrationBuilder.CreateTable(
                name: "trip_categories",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    category_name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    modified_by = table.Column<Guid>(type: "uuid", nullable: true),
                    modified_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_trip_categories", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "materials",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    trip_category_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    material_name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    modified_by = table.Column<Guid>(type: "uuid", nullable: true),
                    modified_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_materials", x => x.id);
                    table.ForeignKey(
                        name: "fk_materials_trip_categories_trip_category_id",
                        column: x => x.trip_category_id,
                        principalSchema: "public",
                        principalTable: "trip_categories",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "trip_category_materials",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    trip_category_id = table.Column<Guid>(type: "uuid", nullable: false),
                    uom_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    modified_by = table.Column<Guid>(type: "uuid", nullable: true),
                    modified_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_trip_category_materials", x => x.id);
                    table.ForeignKey(
                        name: "fk_trip_category_materials_trip_categories_trip_category_id",
                        column: x => x.trip_category_id,
                        principalSchema: "public",
                        principalTable: "trip_categories",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_trip_category_materials_uoms_uom_id",
                        column: x => x.uom_id,
                        principalSchema: "public",
                        principalTable: "uoms",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_materials_trip_category_id",
                schema: "public",
                table: "materials",
                column: "trip_category_id");

            migrationBuilder.CreateIndex(
                name: "ix_trip_category_materials_trip_category_id_uom_id",
                schema: "public",
                table: "trip_category_materials",
                columns: new[] { "trip_category_id", "uom_id" },
                unique: true,
                filter: "is_active = true");

            migrationBuilder.CreateIndex(
                name: "ix_trip_category_materials_uom_id",
                schema: "public",
                table: "trip_category_materials",
                column: "uom_id");

            migrationBuilder.AddForeignKey(
                name: "fk_trips_trip_category_materials_trip_category_material_id",
                schema: "public",
                table: "trips",
                column: "trip_category_material_id",
                principalSchema: "public",
                principalTable: "trip_category_materials",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
