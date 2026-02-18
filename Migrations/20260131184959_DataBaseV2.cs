using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace bookmark_manager_app.Migrations
{
    /// <inheritdoc />
    public partial class DataBaseV2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_bookmark_tags_bookmarks_BookmarkId",
                schema: "bookmark-manager",
                table: "bookmark_tags");

            migrationBuilder.DropForeignKey(
                name: "FK_bookmark_tags_tags_TagId",
                schema: "bookmark-manager",
                table: "bookmark_tags");

            migrationBuilder.DropForeignKey(
                name: "FK_visits_bookmarks_BookmarkId",
                schema: "bookmark-manager",
                table: "visits");

            migrationBuilder.DropPrimaryKey(
                name: "PK_visits",
                schema: "bookmark-manager",
                table: "visits");

            migrationBuilder.DropColumn(
                name: "CreationTime",
                schema: "bookmark-manager",
                table: "bookmark_tags");

            migrationBuilder.DropColumn(
                name: "LastModifiedTime",
                schema: "bookmark-manager",
                table: "bookmark_tags");

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

            migrationBuilder.RenameColumn(
                name: "TagId",
                schema: "bookmark-manager",
                table: "bookmark_tags",
                newName: "TagsTagId");

            migrationBuilder.RenameColumn(
                name: "BookmarkId",
                schema: "bookmark-manager",
                table: "bookmark_tags",
                newName: "BookmarksBookmarkId");

            migrationBuilder.RenameIndex(
                name: "IX_bookmark_tags_TagId",
                schema: "bookmark-manager",
                table: "bookmark_tags",
                newName: "IX_bookmark_tags_TagsTagId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Visits",
                schema: "bookmark-manager",
                table: "Visits",
                column: "VisitId");

            migrationBuilder.AddForeignKey(
                name: "FK_bookmark_tags_bookmarks_BookmarksBookmarkId",
                schema: "bookmark-manager",
                table: "bookmark_tags",
                column: "BookmarksBookmarkId",
                principalSchema: "bookmark-manager",
                principalTable: "bookmarks",
                principalColumn: "BookmarkId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_bookmark_tags_tags_TagsTagId",
                schema: "bookmark-manager",
                table: "bookmark_tags",
                column: "TagsTagId",
                principalSchema: "bookmark-manager",
                principalTable: "tags",
                principalColumn: "TagId",
                onDelete: ReferentialAction.Cascade);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_bookmark_tags_bookmarks_BookmarksBookmarkId",
                schema: "bookmark-manager",
                table: "bookmark_tags");

            migrationBuilder.DropForeignKey(
                name: "FK_bookmark_tags_tags_TagsTagId",
                schema: "bookmark-manager",
                table: "bookmark_tags");

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

            migrationBuilder.RenameColumn(
                name: "TagsTagId",
                schema: "bookmark-manager",
                table: "bookmark_tags",
                newName: "TagId");

            migrationBuilder.RenameColumn(
                name: "BookmarksBookmarkId",
                schema: "bookmark-manager",
                table: "bookmark_tags",
                newName: "BookmarkId");

            migrationBuilder.RenameIndex(
                name: "IX_bookmark_tags_TagsTagId",
                schema: "bookmark-manager",
                table: "bookmark_tags",
                newName: "IX_bookmark_tags_TagId");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CreationTime",
                schema: "bookmark-manager",
                table: "bookmark_tags",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "LastModifiedTime",
                schema: "bookmark-manager",
                table: "bookmark_tags",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_visits",
                schema: "bookmark-manager",
                table: "visits",
                column: "VisitId");

            migrationBuilder.AddForeignKey(
                name: "FK_bookmark_tags_bookmarks_BookmarkId",
                schema: "bookmark-manager",
                table: "bookmark_tags",
                column: "BookmarkId",
                principalSchema: "bookmark-manager",
                principalTable: "bookmarks",
                principalColumn: "BookmarkId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_bookmark_tags_tags_TagId",
                schema: "bookmark-manager",
                table: "bookmark_tags",
                column: "TagId",
                principalSchema: "bookmark-manager",
                principalTable: "tags",
                principalColumn: "TagId",
                onDelete: ReferentialAction.Cascade);

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
    }
}
