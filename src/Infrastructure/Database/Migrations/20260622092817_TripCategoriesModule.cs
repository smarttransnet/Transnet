using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Database.Migrations;

/// <inheritdoc />
public partial class TripCategoriesModule : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "trip_categories",
            schema: "public",
            columns: table => new
            {
                id = table.Column<Guid>(type: "uuid", nullable: false),
                category_name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                created_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                created_by = table.Column<Guid>(type: "uuid", nullable: false),
                modified_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                modified_by = table.Column<Guid>(type: "uuid", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_trip_categories", x => x.id);
            });

        migrationBuilder.CreateTable(
            name: "uoms",
            schema: "public",
            columns: table => new
            {
                id = table.Column<Guid>(type: "uuid", nullable: false),
                uom_code = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                description = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_uoms", x => x.id);
            });

        migrationBuilder.CreateTable(
            name: "materials",
            schema: "public",
            columns: table => new
            {
                id = table.Column<Guid>(type: "uuid", nullable: false),
                trip_category_id = table.Column<Guid>(type: "uuid", nullable: false),
                material_name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                created_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                created_by = table.Column<Guid>(type: "uuid", nullable: false),
                modified_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                modified_by = table.Column<Guid>(type: "uuid", nullable: true)
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
                material_id = table.Column<Guid>(type: "uuid", nullable: false),
                uom_id = table.Column<Guid>(type: "uuid", nullable: false),
                is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                created_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                created_by = table.Column<Guid>(type: "uuid", nullable: false),
                modified_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                modified_by = table.Column<Guid>(type: "uuid", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_trip_category_materials", x => x.id);
                table.ForeignKey(
                    name: "fk_trip_category_materials_materials_material_id",
                    column: x => x.material_id,
                    principalSchema: "public",
                    principalTable: "materials",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
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
            name: "ix_trip_category_materials_material_id",
            schema: "public",
            table: "trip_category_materials",
            column: "material_id");

        migrationBuilder.CreateIndex(
            name: "ix_trip_category_materials_trip_category_id_material_id_uom_id",
            schema: "public",
            table: "trip_category_materials",
            columns: new[] { "trip_category_id", "material_id", "uom_id" },
            unique: true,
            filter: "is_active = true");

        migrationBuilder.CreateIndex(
            name: "ix_trip_category_materials_uom_id",
            schema: "public",
            table: "trip_category_materials",
            column: "uom_id");

        migrationBuilder.CreateIndex(
            name: "ix_uoms_uom_code",
            schema: "public",
            table: "uoms",
            column: "uom_code",
            unique: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "trip_category_materials",
            schema: "public");

        migrationBuilder.DropTable(
            name: "materials",
            schema: "public");

        migrationBuilder.DropTable(
            name: "uoms",
            schema: "public");

        migrationBuilder.DropTable(
            name: "trip_categories",
            schema: "public");
    }
}
