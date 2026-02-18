using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace bookmark_manager_app.Migrations
{
    /// <inheritdoc />
    public partial class DataBaseV6 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_visits_BookmarkId",
                schema: "bookmark-manager",
                table: "visits");

            migrationBuilder.CreateIndex(
                name: "IX_visits_BookmarkId_VisitTime",
                schema: "bookmark-manager",
                table: "visits",
                columns: new[] { "BookmarkId", "VisitTime" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_visits_BookmarkId_VisitTime",
                schema: "bookmark-manager",
                table: "visits");

            migrationBuilder.CreateIndex(
                name: "IX_visits_BookmarkId",
                schema: "bookmark-manager",
                table: "visits",
                column: "BookmarkId");
        }
    }
}
