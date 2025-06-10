using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CareerGuidance.Api.Migrations
{
    /// <inheritdoc />
    public partial class progressroadmapdelete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "6dc6528a-b280-4770-9eae-82671ee81ef7",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAED4P5LmSnAK8lgiJxcRz18sUwZsF+meBw24C7rGiCd0Xy0aqypIID3Y6JO3OcoDD0w==");

            migrationBuilder.CreateIndex(
                name: "IX_progressBar_RoadmapId",
                table: "progressBar",
                column: "RoadmapId");

            migrationBuilder.AddForeignKey(
                name: "FK_progressBar_ParsedRoadmaps_RoadmapId",
                table: "progressBar",
                column: "RoadmapId",
                principalTable: "ParsedRoadmaps",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_progressBar_ParsedRoadmaps_RoadmapId",
                table: "progressBar");

            migrationBuilder.DropIndex(
                name: "IX_progressBar_RoadmapId",
                table: "progressBar");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "6dc6528a-b280-4770-9eae-82671ee81ef7",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEFe2sUjfrmgO9moRZB8YWp+rAUvzsrJaWB320un74FkuWebxqSRDpntwdhZvvTVYVQ==");
        }
    }
}
