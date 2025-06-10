using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CareerGuidance.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddCarouselTablesAndFAQTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FAQs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Question = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Answer = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FAQs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NewCarouselSection",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    newCarouselSection = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NewCarouselSection", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DetailsCarouselSection",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    carouselSection = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    carouselState = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    carouselTitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    carouselDes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    carouselImg = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    carouselUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CarouselSectionId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DetailsCarouselSection", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DetailsCarouselSection_NewCarouselSection_CarouselSectionId",
                        column: x => x.CarouselSectionId,
                        principalTable: "NewCarouselSection",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "6dc6528a-b280-4770-9eae-82671ee81ef7",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEDmLIKdM4hDNUmWmMEj3mB9SVxSsg3gAHB6ikC8yGfmZZhL1Px5DeUF5YPecbavOCg==");

            migrationBuilder.CreateIndex(
                name: "IX_DetailsCarouselSection_CarouselSectionId",
                table: "DetailsCarouselSection",
                column: "CarouselSectionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DetailsCarouselSection");

            migrationBuilder.DropTable(
                name: "FAQs");

            migrationBuilder.DropTable(
                name: "NewCarouselSection");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "6dc6528a-b280-4770-9eae-82671ee81ef7",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEGkPnu4/xdZ2pSFenzhBVEna8KFh6njazA6GIVzkyIstIjuHgcFqjtQ29t4ySM8gdQ==");
        }
    }
}
