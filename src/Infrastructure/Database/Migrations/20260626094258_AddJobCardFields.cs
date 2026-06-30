using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddJobCardFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "authorized_by",
                schema: "public",
                table: "work_orders",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "checked_by_driver",
                schema: "public",
                table: "work_orders",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "checked_by_mechanic",
                schema: "public",
                table: "work_orders",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "driver_name",
                schema: "public",
                table: "work_orders",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "job_type",
                schema: "public",
                table: "work_orders",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "prepared_by",
                schema: "public",
                table: "work_orders",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "end_time",
                schema: "public",
                table: "work_order_items",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "mechanic_name",
                schema: "public",
                table: "work_order_items",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "remarks",
                schema: "public",
                table: "work_order_items",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "start_time",
                schema: "public",
                table: "work_order_items",
                type: "timestamp with time zone",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "authorized_by",
                schema: "public",
                table: "work_orders");

            migrationBuilder.DropColumn(
                name: "checked_by_driver",
                schema: "public",
                table: "work_orders");

            migrationBuilder.DropColumn(
                name: "checked_by_mechanic",
                schema: "public",
                table: "work_orders");

            migrationBuilder.DropColumn(
                name: "driver_name",
                schema: "public",
                table: "work_orders");

            migrationBuilder.DropColumn(
                name: "job_type",
                schema: "public",
                table: "work_orders");

            migrationBuilder.DropColumn(
                name: "prepared_by",
                schema: "public",
                table: "work_orders");

            migrationBuilder.DropColumn(
                name: "end_time",
                schema: "public",
                table: "work_order_items");

            migrationBuilder.DropColumn(
                name: "mechanic_name",
                schema: "public",
                table: "work_order_items");

            migrationBuilder.DropColumn(
                name: "remarks",
                schema: "public",
                table: "work_order_items");

            migrationBuilder.DropColumn(
                name: "start_time",
                schema: "public",
                table: "work_order_items");
        }
    }
}
