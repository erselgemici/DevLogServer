using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DevLog.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class mig_translations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BlogTranslations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BlogId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LanguageCode = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlogTranslations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BlogTranslations_Blogs_BlogId",
                        column: x => x.BlogId,
                        principalTable: "Blogs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CategoryTranslations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LanguageCode = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    CategoryName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryTranslations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CategoryTranslations_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BlogTranslations_BlogId_LanguageCode",
                table: "BlogTranslations",
                columns: new[] { "BlogId", "LanguageCode" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CategoryTranslations_CategoryId_LanguageCode",
                table: "CategoryTranslations",
                columns: new[] { "CategoryId", "LanguageCode" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BlogTranslations");

            migrationBuilder.DropTable(
                name: "CategoryTranslations");
        }
    }
}
