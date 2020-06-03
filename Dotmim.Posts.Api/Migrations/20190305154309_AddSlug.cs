using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Dotmim.Posts.Api.Migrations
{
    public partial class AddSlug : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: new Guid("b81ba09f-6ca7-45c3-8319-1f002ae36c49"));

            migrationBuilder.InsertData(
                table: "Posts",
                columns: new[] { "Id", "BlogContent", "Excerpt", "IsPublished", "LastModified", "PubDate", "Slug", "Title" },
                values: new object[] { new Guid("53430b5c-d705-4984-88c6-7131de866b1a"), "Welcome !", null, true, new DateTime(2019, 3, 5, 16, 43, 8, 743, DateTimeKind.Local).AddTicks(778), new DateTime(2019, 3, 5, 16, 43, 8, 741, DateTimeKind.Local).AddTicks(8388), "welcome", "Welcome" });

            migrationBuilder.UpdateData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: 1,
                column: "PostId",
                value: new Guid("53430b5c-d705-4984-88c6-7131de866b1a"));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: new Guid("53430b5c-d705-4984-88c6-7131de866b1a"));

            migrationBuilder.InsertData(
                table: "Posts",
                columns: new[] { "Id", "BlogContent", "Excerpt", "IsPublished", "LastModified", "PubDate", "Slug", "Title" },
                values: new object[] { new Guid("b81ba09f-6ca7-45c3-8319-1f002ae36c49"), "Welcome !", null, true, new DateTime(2019, 3, 5, 14, 55, 19, 356, DateTimeKind.Local).AddTicks(2731), new DateTime(2019, 3, 5, 14, 55, 19, 355, DateTimeKind.Local).AddTicks(902), null, "Welcome" });

            migrationBuilder.UpdateData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: 1,
                column: "PostId",
                value: new Guid("b81ba09f-6ca7-45c3-8319-1f002ae36c49"));
        }
    }
}
