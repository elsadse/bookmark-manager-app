using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace bookmark_manager_app.Migrations
{
    /// <inheritdoc />
    public partial class DataBaseV4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Visits_bookmarks_BookmarkId",
                schema: "bookmark-manager",
                table: "Visits");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Visits",
                schema: "bookmark-manager",
                table: "Visits");

            migrationBuilder.RenameTable(
                name: "Visits",
                schema: "bookmark-manager",
                newName: "visits",
                newSchema: "bookmark-manager");

            migrationBuilder.RenameIndex(
                name: "IX_Visits_BookmarkId",
                schema: "bookmark-manager",
                table: "visits",
                newName: "IX_visits_BookmarkId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_visits",
                schema: "bookmark-manager",
                table: "visits",
                column: "VisitId");

            migrationBuilder.AddForeignKey(
                name: "FK_visits_bookmarks_BookmarkId",
                schema: "bookmark-manager",
                table: "visits",
                column: "BookmarkId",
                principalSchema: "bookmark-manager",
                principalTable: "bookmarks",
                principalColumn: "BookmarkId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_visits_bookmarks_BookmarkId",
                schema: "bookmark-manager",
                table: "visits");

            migrationBuilder.DropPrimaryKey(
                name: "PK_visits",
                schema: "bookmark-manager",
                table: "visits");

            migrationBuilder.RenameTable(
                name: "visits",
                schema: "bookmark-manager",
                newName: "Visits",
                newSchema: "bookmark-manager");

            migrationBuilder.RenameIndex(
                name: "IX_visits_BookmarkId",
                schema: "bookmark-manager",
                table: "Visits",
                newName: "IX_Visits_BookmarkId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Visits",
                schema: "bookmark-manager",
                table: "Visits",
                column: "VisitId");

            migrationBuilder.AddForeignKey(
                name: "FK_Visits_bookmarks_BookmarkId",
                schema: "bookmark-manager",
                table: "Visits",
                column: "BookmarkId",
                principalSchema: "bookmark-manager",
                principalTable: "bookmarks",
                principalColumn: "BookmarkId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
