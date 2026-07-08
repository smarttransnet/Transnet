using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class RemoveRegistrationAddChassis : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "registration_number",
                schema: "public",
                table: "vehicles");

            migrationBuilder.AddColumn<string>(
                name: "chassis_number",
                schema: "public",
                table: "vehicles",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "chassis_number",
                schema: "public",
                table: "vehicles");

            migrationBuilder.AddColumn<string>(
                name: "registration_number",
                schema: "public",
                table: "vehicles",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");
        }
    }
}
