using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class RemoveMaterialField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_trip_category_materials_materials_material_id",
                schema: "public",
                table: "trip_category_materials");

            migrationBuilder.DropIndex(
                name: "ix_trip_category_materials_material_id",
                schema: "public",
                table: "trip_category_materials");

            migrationBuilder.DropIndex(
                name: "ix_trip_category_materials_trip_category_id_material_id_uom_id",
                schema: "public",
                table: "trip_category_materials");

            migrationBuilder.DropColumn(
                name: "material_id",
                schema: "public",
                table: "trip_category_materials");

            migrationBuilder.CreateIndex(
                name: "ix_trip_category_materials_trip_category_id_uom_id",
                schema: "public",
                table: "trip_category_materials",
                columns: new[] { "trip_category_id", "uom_id" },
                unique: true,
                filter: "is_active = true");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_trip_category_materials_trip_category_id_uom_id",
                schema: "public",
                table: "trip_category_materials");

            migrationBuilder.AddColumn<Guid>(
                name: "material_id",
                schema: "public",
                table: "trip_category_materials",
                type: "uuid",
                nullable: false,
                defaultValue: Guid.Empty);

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

            migrationBuilder.AddForeignKey(
                name: "fk_trip_category_materials_materials_material_id",
                schema: "public",
                table: "trip_category_materials",
                column: "material_id",
                principalSchema: "public",
                principalTable: "materials",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
