using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace bookmark_manager_app.Migrations
{
    /// <inheritdoc />
    public partial class DataBaseV1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                schema: "bookmark",
                table: "users",
                newName: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "id",
                schema: "bookmark",
                table: "users",
                newName: "Id");
        }
    }
}
