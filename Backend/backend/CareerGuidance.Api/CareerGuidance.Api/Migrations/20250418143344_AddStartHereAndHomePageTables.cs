using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CareerGuidance.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddStartHereAndHomePageTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ImportantStartHere",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    startHereImportanceTitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    startHereImportanceDes = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImportantStartHere", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "IntroductionHomePage",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IntroductionHomePage", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "IntroductionStartHere",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    startHereIntroTitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    startHereIntroDes = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IntroductionStartHere", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "6dc6528a-b280-4770-9eae-82671ee81ef7",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEGkPnu4/xdZ2pSFenzhBVEna8KFh6njazA6GIVzkyIstIjuHgcFqjtQ29t4ySM8gdQ==");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ImportantStartHere");

            migrationBuilder.DropTable(
                name: "IntroductionHomePage");

            migrationBuilder.DropTable(
                name: "IntroductionStartHere");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "6dc6528a-b280-4770-9eae-82671ee81ef7",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEMl+uUBoknpb9YW/QzPAo59r62GkEaKCWBIim11sR/2D8AeZK2gCqKuYaVIc9PylXw==");
        }
    }
}
