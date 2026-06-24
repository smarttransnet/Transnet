#pragma warning disable IDE0161
using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddTripCategoryMaterialToTrip : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "trip_category_material_id",
                schema: "public",
                table: "trips",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "ix_trips_trip_category_material_id",
                schema: "public",
                table: "trips",
                column: "trip_category_material_id");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_trips_trip_category_materials_trip_category_material_id",
                schema: "public",
                table: "trips");

            migrationBuilder.DropIndex(
                name: "ix_trips_trip_category_material_id",
                schema: "public",
                table: "trips");

            migrationBuilder.DropColumn(
                name: "trip_category_material_id",
                schema: "public",
                table: "trips");
        }
    }
}
