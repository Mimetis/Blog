using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Dotmim.Posts.Api.Migrations
{
    public partial class InitDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    ID = table.Column<Guid>(nullable: false),
                    PostID = table.Column<Guid>(nullable: false),
                    Author = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    CommentContent = table.Column<string>(nullable: true),
                    PubDate = table.Column<DateTime>(nullable: true),
                    IsAdmin = table.Column<bool>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Posts",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false, defaultValueSql: "(newid())"),
                    Title = table.Column<string>(maxLength: 150, nullable: false),
                    Slug = table.Column<string>(maxLength: 150, nullable: true),
                    Excerpt = table.Column<string>(nullable: true),
                    BlogContent = table.Column<string>(nullable: false),
                    PubDate = table.Column<DateTime>(nullable: true),
                    LastModified = table.Column<DateTime>(nullable: true),
                    IsPublished = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Posts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tags",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Title = table.Column<string>(maxLength: 150, nullable: false),
                    PostId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tags", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tags_Posts_PostId",
                        column: x => x.PostId,
                        principalTable: "Posts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Posts",
                columns: new[] { "Id", "BlogContent", "Excerpt", "IsPublished", "LastModified", "PubDate", "Slug", "Title" },
                values: new object[] { new Guid("b81ba09f-6ca7-45c3-8319-1f002ae36c49"), "Welcome !", null, true, new DateTime(2019, 3, 5, 14, 55, 19, 356, DateTimeKind.Local).AddTicks(2731), new DateTime(2019, 3, 5, 14, 55, 19, 355, DateTimeKind.Local).AddTicks(902), null, "Welcome" });

            migrationBuilder.InsertData(
                table: "Tags",
                columns: new[] { "Id", "PostId", "Title" },
                values: new object[] { 1, new Guid("b81ba09f-6ca7-45c3-8319-1f002ae36c49"), "#Welcome" });

            migrationBuilder.CreateIndex(
                name: "IX_Tags_PostId",
                table: "Tags",
                column: "PostId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.DropTable(
                name: "Tags");

            migrationBuilder.DropTable(
                name: "Posts");
        }
    }
}
