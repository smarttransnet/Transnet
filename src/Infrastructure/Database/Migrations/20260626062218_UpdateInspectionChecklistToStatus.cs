using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Database.Migrations;

/// <inheritdoc />
public partial class UpdateInspectionChecklistToStatus : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "is_passed",
            schema: "public",
            table: "inspection_results");

        migrationBuilder.DropColumn(
            name: "applicable_vehicle_types",
            schema: "public",
            table: "inspection_checklists");

        migrationBuilder.DropColumn(
            name: "inspection_type",
            schema: "public",
            table: "inspection_checklists");

        migrationBuilder.AddColumn<string>(
            name: "status",
            schema: "public",
            table: "inspection_results",
            type: "character varying(50)",
            maxLength: 50,
            nullable: false,
            defaultValue: "");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "status",
            schema: "public",
            table: "inspection_results");

        migrationBuilder.AddColumn<bool>(
            name: "is_passed",
            schema: "public",
            table: "inspection_results",
            type: "boolean",
            nullable: false,
            defaultValue: false);

        migrationBuilder.AddColumn<string>(
            name: "applicable_vehicle_types",
            schema: "public",
            table: "inspection_checklists",
            type: "character varying(500)",
            maxLength: 500,
            nullable: false,
            defaultValue: "");

        migrationBuilder.AddColumn<int>(
            name: "inspection_type",
            schema: "public",
            table: "inspection_checklists",
            type: "integer",
            nullable: false,
            defaultValue: 0);
    }
}
