using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Database.Migrations;

/// <inheritdoc />
public partial class UpdateCustomFieldDefinition : Migration
{
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "default_value",
                table: "custom_field_definitions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "field_type",
                table: "custom_field_definitions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "validation_regex",
                table: "custom_field_definitions",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "default_value",
                table: "custom_field_definitions");

            migrationBuilder.DropColumn(
                name: "field_type",
                table: "custom_field_definitions");

            migrationBuilder.DropColumn(
                name: "validation_regex",
                table: "custom_field_definitions");
        }
    }
