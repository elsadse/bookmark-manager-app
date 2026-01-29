using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace bookmark_manager_app.Migrations
{
    /// <inheritdoc />
    public partial class InitialDataBase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "bookmark_manager");

            migrationBuilder.CreateTable(
                name: "tags",
                schema: "bookmark_manager",
                columns: table => new
                {
                    TagId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tags", x => x.TagId);
                });

            migrationBuilder.CreateTable(
                name: "users",
                schema: "bookmark_manager",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FullName = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, defaultValueSql: "NOW()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "bookmarks",
                schema: "bookmark_manager",
                columns: table => new
                {
                    BookmarkId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Url = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "character varying(280)", maxLength: 280, nullable: false),
                    IsPinned = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    IsArchived = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, defaultValueSql: "NOW()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bookmarks", x => x.BookmarkId);
                    table.ForeignKey(
                        name: "FK_bookmarks_users_UserId",
                        column: x => x.UserId,
                        principalSchema: "bookmark_manager",
                        principalTable: "users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "bookmark_tags",
                schema: "bookmark_manager",
                columns: table => new
                {
                    BookmarkId = table.Column<int>(type: "integer", nullable: false),
                    TagId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bookmark_tags", x => new { x.BookmarkId, x.TagId });
                    table.ForeignKey(
                        name: "FK_bookmark_tags_bookmarks_BookmarkId",
                        column: x => x.BookmarkId,
                        principalSchema: "bookmark_manager",
                        principalTable: "bookmarks",
                        principalColumn: "BookmarkId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_bookmark_tags_tags_TagId",
                        column: x => x.TagId,
                        principalSchema: "bookmark_manager",
                        principalTable: "tags",
                        principalColumn: "TagId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "visits",
                schema: "bookmark_manager",
                columns: table => new
                {
                    VisitId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    BookmarkId = table.Column<int>(type: "integer", nullable: false),
                    VisitDateAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_visits", x => x.VisitId);
                    table.ForeignKey(
                        name: "FK_visits_bookmarks_BookmarkId",
                        column: x => x.BookmarkId,
                        principalSchema: "bookmark_manager",
                        principalTable: "bookmarks",
                        principalColumn: "BookmarkId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_bookmark_tags_TagId",
                schema: "bookmark_manager",
                table: "bookmark_tags",
                column: "TagId");

            migrationBuilder.CreateIndex(
                name: "IX_bookmarks_UserId_Title",
                schema: "bookmark_manager",
                table: "bookmarks",
                columns: new[] { "UserId", "Title" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_bookmarks_UserId_Url",
                schema: "bookmark_manager",
                table: "bookmarks",
                columns: new[] { "UserId", "Url" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_tags_Name",
                schema: "bookmark_manager",
                table: "tags",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_users_Email",
                schema: "bookmark_manager",
                table: "users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_visits_BookmarkId",
                schema: "bookmark_manager",
                table: "visits",
                column: "BookmarkId");

            migrationBuilder.CreateIndex(
                name: "IX_visits_VisitDateAt",
                schema: "bookmark_manager",
                table: "visits",
                column: "VisitDateAt");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "bookmark_tags",
                schema: "bookmark_manager");

            migrationBuilder.DropTable(
                name: "visits",
                schema: "bookmark_manager");

            migrationBuilder.DropTable(
                name: "tags",
                schema: "bookmark_manager");

            migrationBuilder.DropTable(
                name: "bookmarks",
                schema: "bookmark_manager");

            migrationBuilder.DropTable(
                name: "users",
                schema: "bookmark_manager");
        }
    }
}
