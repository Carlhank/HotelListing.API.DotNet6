#nullable disable

using Microsoft.EntityFrameworkCore.Migrations;

namespace HotelListing.API.Data.Migrations
{
    public partial class AddedDefaultRoles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "455d95c8-1907-4c2e-bd09-1776884a5834", "6911b9f4-0a78-4ceb-b45d-a09ae21fdf66", "Administrator", "ADMINISTRATOR" },
                    { "ee925574-c910-4cb8-9368-33d8b4130c55", "4edc86f1-70a9-470c-905f-4f7103848a25", "User", "USER" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "455d95c8-1907-4c2e-bd09-1776884a5834");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ee925574-c910-4cb8-9368-33d8b4130c55");
        }
    }
}
