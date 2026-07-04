using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class HardDeleteAllCategoriesAndUoms : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("TRUNCATE TABLE trip_category_materials CASCADE;");
            migrationBuilder.Sql("TRUNCATE TABLE trip_categories CASCADE;");
            migrationBuilder.Sql("TRUNCATE TABLE uoms CASCADE;");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Intentionally left empty.
        }
    }
}
