using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace bookmark_manager_app.Migrations
{
    /// <inheritdoc />
    public partial class updateBookmarksTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_bookmark",
                schema: "bookmark",
                table: "bookmark");

            migrationBuilder.RenameTable(
                name: "bookmark",
                schema: "bookmark",
                newName: "bookmarks",
                newSchema: "bookmark");

            migrationBuilder.RenameIndex(
                name: "IX_bookmark_user_id",
                schema: "bookmark",
                table: "bookmarks",
                newName: "IX_bookmarks_user_id");

            migrationBuilder.RenameIndex(
                name: "IX_bookmark_url",
                schema: "bookmark",
                table: "bookmarks",
                newName: "IX_bookmarks_url");

            migrationBuilder.RenameIndex(
                name: "IX_bookmark_created_at",
                schema: "bookmark",
                table: "bookmarks",
                newName: "IX_bookmarks_created_at");

            migrationBuilder.AddPrimaryKey(
                name: "PK_bookmarks",
                schema: "bookmark",
                table: "bookmarks",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_bookmarks_users_user_id",
                schema: "bookmark",
                table: "bookmarks",
                column: "user_id",
                principalSchema: "bookmark",
                principalTable: "users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_bookmarks_users_user_id",
                schema: "bookmark",
                table: "bookmarks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_bookmarks",
                schema: "bookmark",
                table: "bookmarks");

            migrationBuilder.RenameTable(
                name: "bookmarks",
                schema: "bookmark",
                newName: "bookmark",
                newSchema: "bookmark");

            migrationBuilder.RenameIndex(
                name: "IX_bookmarks_user_id",
                schema: "bookmark",
                table: "bookmark",
                newName: "IX_bookmark_user_id");

            migrationBuilder.RenameIndex(
                name: "IX_bookmarks_url",
                schema: "bookmark",
                table: "bookmark",
                newName: "IX_bookmark_url");

            migrationBuilder.RenameIndex(
                name: "IX_bookmarks_created_at",
                schema: "bookmark",
                table: "bookmark",
                newName: "IX_bookmark_created_at");

            migrationBuilder.AddPrimaryKey(
                name: "PK_bookmark",
                schema: "bookmark",
                table: "bookmark",
                column: "Id");
        }
    }
}
