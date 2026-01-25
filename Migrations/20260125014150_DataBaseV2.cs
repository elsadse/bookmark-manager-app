using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace bookmark_manager_app.Migrations
{
    /// <inheritdoc />
    public partial class DataBaseV2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_bookmarks_created_at",
                schema: "bookmark",
                table: "bookmarks");

            migrationBuilder.DropIndex(
                name: "IX_bookmarks_url",
                schema: "bookmark",
                table: "bookmarks");

            migrationBuilder.DropIndex(
                name: "IX_bookmarks_user_id",
                schema: "bookmark",
                table: "bookmarks");

            migrationBuilder.DropColumn(
                name: "tags",
                schema: "bookmark",
                table: "bookmarks");

            migrationBuilder.DropColumn(
                name: "visit_count",
                schema: "bookmark",
                table: "bookmarks");

            migrationBuilder.DropColumn(
                name: "visited_last_at",
                schema: "bookmark",
                table: "bookmarks");

            migrationBuilder.RenameColumn(
                name: "username",
                schema: "bookmark",
                table: "users",
                newName: "full_name");

            migrationBuilder.RenameColumn(
                name: "id",
                schema: "bookmark",
                table: "users",
                newName: "user_id");

            migrationBuilder.RenameColumn(
                name: "Id",
                schema: "bookmark",
                table: "bookmarks",
                newName: "bookmark_id");

            migrationBuilder.AddColumn<DateTime>(
                name: "created_at",
                schema: "bookmark",
                table: "users",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP");

            migrationBuilder.AddColumn<DateTime>(
                name: "updated_at",
                schema: "bookmark",
                table: "users",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "is_pinned",
                schema: "bookmark",
                table: "bookmarks",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<bool>(
                name: "is_archived",
                schema: "bookmark",
                table: "bookmarks",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AddColumn<DateTime>(
                name: "update_at",
                schema: "bookmark",
                table: "bookmarks",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "tags",
                schema: "bookmark",
                columns: table => new
                {
                    tag_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tags", x => x.tag_id);
                });

            migrationBuilder.CreateTable(
                name: "visits",
                schema: "bookmark",
                columns: table => new
                {
                    visit_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    bookmark_id = table.Column<int>(type: "integer", nullable: false),
                    visit_date_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NULL")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_visits", x => x.visit_id);
                    table.ForeignKey(
                        name: "FK_visits_bookmarks_bookmark_id",
                        column: x => x.bookmark_id,
                        principalSchema: "bookmark",
                        principalTable: "bookmarks",
                        principalColumn: "bookmark_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "bookmark_tags",
                schema: "bookmark",
                columns: table => new
                {
                    BookmarkTagId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    BookmarkId = table.Column<int>(type: "integer", nullable: false),
                    TagId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bookmark_tags", x => x.BookmarkTagId);
                    table.ForeignKey(
                        name: "FK_bookmark_tags_bookmarks_BookmarkId",
                        column: x => x.BookmarkId,
                        principalSchema: "bookmark",
                        principalTable: "bookmarks",
                        principalColumn: "bookmark_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_bookmark_tags_tags_TagId",
                        column: x => x.TagId,
                        principalSchema: "bookmark",
                        principalTable: "tags",
                        principalColumn: "tag_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_bookmarks_user_id_title",
                schema: "bookmark",
                table: "bookmarks",
                columns: new[] { "user_id", "title" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_bookmarks_user_id_url",
                schema: "bookmark",
                table: "bookmarks",
                columns: new[] { "user_id", "url" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_bookmark_tags_BookmarkId_TagId",
                schema: "bookmark",
                table: "bookmark_tags",
                columns: new[] { "BookmarkId", "TagId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_bookmark_tags_TagId",
                schema: "bookmark",
                table: "bookmark_tags",
                column: "TagId");

            migrationBuilder.CreateIndex(
                name: "IX_tags_name",
                schema: "bookmark",
                table: "tags",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_visits_bookmark_id",
                schema: "bookmark",
                table: "visits",
                column: "bookmark_id");

            migrationBuilder.CreateIndex(
                name: "IX_visits_visit_date_at",
                schema: "bookmark",
                table: "visits",
                column: "visit_date_at");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "bookmark_tags",
                schema: "bookmark");

            migrationBuilder.DropTable(
                name: "visits",
                schema: "bookmark");

            migrationBuilder.DropTable(
                name: "tags",
                schema: "bookmark");

            migrationBuilder.DropIndex(
                name: "IX_bookmarks_user_id_title",
                schema: "bookmark",
                table: "bookmarks");

            migrationBuilder.DropIndex(
                name: "IX_bookmarks_user_id_url",
                schema: "bookmark",
                table: "bookmarks");

            migrationBuilder.DropColumn(
                name: "created_at",
                schema: "bookmark",
                table: "users");

            migrationBuilder.DropColumn(
                name: "updated_at",
                schema: "bookmark",
                table: "users");

            migrationBuilder.DropColumn(
                name: "update_at",
                schema: "bookmark",
                table: "bookmarks");

            migrationBuilder.RenameColumn(
                name: "full_name",
                schema: "bookmark",
                table: "users",
                newName: "username");

            migrationBuilder.RenameColumn(
                name: "user_id",
                schema: "bookmark",
                table: "users",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "bookmark_id",
                schema: "bookmark",
                table: "bookmarks",
                newName: "Id");

            migrationBuilder.AlterColumn<bool>(
                name: "is_pinned",
                schema: "bookmark",
                table: "bookmarks",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "is_archived",
                schema: "bookmark",
                table: "bookmarks",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldDefaultValue: false);

            migrationBuilder.AddColumn<List<string>>(
                name: "tags",
                schema: "bookmark",
                table: "bookmarks",
                type: "text[]",
                nullable: false);

            migrationBuilder.AddColumn<int>(
                name: "visit_count",
                schema: "bookmark",
                table: "bookmarks",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "visited_last_at",
                schema: "bookmark",
                table: "bookmarks",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP");

            migrationBuilder.CreateIndex(
                name: "IX_bookmarks_created_at",
                schema: "bookmark",
                table: "bookmarks",
                column: "created_at");

            migrationBuilder.CreateIndex(
                name: "IX_bookmarks_url",
                schema: "bookmark",
                table: "bookmarks",
                column: "url");

            migrationBuilder.CreateIndex(
                name: "IX_bookmarks_user_id",
                schema: "bookmark",
                table: "bookmarks",
                column: "user_id");
        }
    }
}
