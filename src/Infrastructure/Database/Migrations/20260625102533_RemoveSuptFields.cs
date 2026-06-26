using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Database.Migrations;

/// <inheritdoc />
public partial class RemoveSuptFields : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "supt_doc_path",
            schema: "public",
            table: "trips");

        migrationBuilder.DropColumn(
            name: "supt_no",
            schema: "public",
            table: "trips");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<string>(
            name: "supt_doc_path",
            schema: "public",
            table: "trips",
            type: "character varying(1000)",
            maxLength: 1000,
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "supt_no",
            schema: "public",
            table: "trips",
            type: "character varying(100)",
            maxLength: 100,
            nullable: true);
    }
}
