using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class RemoveCategoryMappingSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM trip_category_materials WHERE id NOT IN (SELECT trip_category_material_id FROM trips WHERE trip_category_material_id IS NOT NULL);");
            migrationBuilder.Sql("DELETE FROM trip_categories WHERE id NOT IN (SELECT trip_category_id FROM trip_category_materials) AND id NOT IN (SELECT trip_category_id FROM materials WHERE trip_category_id IS NOT NULL);");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Method intentionally left empty.
        }
    }
}
