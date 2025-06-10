using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CareerGuidance.Api.Migrations
{
    /// <inheritdoc />
    public partial class CarouselStateIsNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "carouselState",
                table: "DetailsCarouselSection",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "6dc6528a-b280-4770-9eae-82671ee81ef7",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEFe2sUjfrmgO9moRZB8YWp+rAUvzsrJaWB320un74FkuWebxqSRDpntwdhZvvTVYVQ==");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "carouselState",
                table: "DetailsCarouselSection",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "6dc6528a-b280-4770-9eae-82671ee81ef7",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEJeZMiqHPL9o/V4qEvAx+g84JqhFRDdQP9Kndc2dXntTwWZ/cwzyYw4f3wLBZI8rvg==");
        }
    }
}
