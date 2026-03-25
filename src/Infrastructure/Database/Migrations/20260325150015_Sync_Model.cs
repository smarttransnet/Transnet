using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class Sync_Model : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_drivers_email",
                schema: "public",
                table: "drivers");

            migrationBuilder.CreateIndex(
                name: "ix_drivers_email",
                schema: "public",
                table: "drivers",
                column: "email",
                unique: true,
                filter: "email IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_drivers_email",
                schema: "public",
                table: "drivers");

            migrationBuilder.CreateIndex(
                name: "ix_drivers_email",
                schema: "public",
                table: "drivers",
                column: "email",
                unique: true,
                filter: "[email] IS NOT NULL");
        }
    }
}
