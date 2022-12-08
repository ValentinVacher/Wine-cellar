using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Winecelar.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Appelations",
                columns: table => new
                {
                    AppelationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppelationName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    KeepMin = table.Column<int>(type: "int", nullable: false),
                    KeepMax = table.Column<int>(type: "int", nullable: false),
                    Color = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Appelations", x => x.AppelationId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "Cellars",
                columns: table => new
                {
                    CellarId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NbDrawerMax = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cellars", x => x.CellarId);
                    table.ForeignKey(
                        name: "FK_Cellars_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Drawers",
                columns: table => new
                {
                    DrawerId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Index = table.Column<int>(type: "int", nullable: false),
                    NbBottleMax = table.Column<int>(type: "int", nullable: false),
                    CellarId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Drawers", x => x.DrawerId);
                    table.ForeignKey(
                        name: "FK_Drawers_Cellars_CellarId",
                        column: x => x.CellarId,
                        principalTable: "Cellars",
                        principalColumn: "CellarId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Wines",
                columns: table => new
                {
                    WineId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false),
                    DrawerId = table.Column<int>(type: "int", nullable: false),
                    PictureName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Color = table.Column<int>(type: "int", nullable: false),
                    AppelationId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wines", x => x.WineId);
                    table.ForeignKey(
                        name: "FK_Wines_Appelations_AppelationId",
                        column: x => x.AppelationId,
                        principalTable: "Appelations",
                        principalColumn: "AppelationId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Wines_Drawers_DrawerId",
                        column: x => x.DrawerId,
                        principalTable: "Drawers",
                        principalColumn: "DrawerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Appelations",
                columns: new[] { "AppelationId", "AppelationName", "Color", "KeepMax", "KeepMin" },
                values: new object[,]
                {
                    { 1, "Bordeaux", 0, 10, 5 },
                    { 2, "Loire", 0, 6, 4 },
                    { 3, "Bordeaux, Grands crus", 0, 20, 10 },
                    { 4, "Sud-Ouest", 0, 10, 5 },
                    { 5, "Languedoc & Provence", 0, 8, 5 },
                    { 6, "Côtes du Rhône", 0, 6, 4 },
                    { 7, "Cotes du Rhônes, Grands Crus", 0, 20, 10 },
                    { 8, "Beaujolais", 0, 5, 4 },
                    { 9, "Beaujolais, Crus", 0, 8, 5 },
                    { 10, "Bourgogne, Saône-et-Loire", 0, 10, 5 },
                    { 11, "Bourgogne, Côte-d'Or", 0, 20, 10 },
                    { 12, "Loire, Sec", 2, 4, 3 },
                    { 13, "Loire, moelleux et liquoreux", 2, 20, 10 },
                    { 14, "Bordeaux, sec", 2, 8, 5 },
                    { 15, "Bordeaux, liquoreux", 2, 20, 15 },
                    { 16, "Sud-Ouest, sec", 2, 5, 4 },
                    { 17, "Sud-Ouest, liquoreux", 2, 15, 10 },
                    { 18, "Languedoc & Provence", 2, 3, 3 },
                    { 19, "Côtes du Rhône", 2, 4, 3 },
                    { 20, "Bourgogne, Saône-et-Loire", 2, 4, 4 },
                    { 21, "Bourgogne, Côte-d'Or", 2, 10, 7 },
                    { 22, "Jura", 2, 20, 8 },
                    { 23, "Jura", 2, 20, 8 },
                    { 24, "Alsace", 2, 5, 4 },
                    { 25, "Languedoc", 1, 4, 3 },
                    { 26, "Provence", 1, 3, 3 },
                    { 27, "Rhône", 1, 2, 2 }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "DateOfBirth", "Email", "FirstName", "LastName", "Password" },
                values: new object[,]
                {
                    { 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "test@test.com", "G", "G", "test" },
                    { 2, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "test2@test.com", "G2", "G2", "test2" },
                    { 3, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "test3@test.com", "G3", "G3", "test3" }
                });

            migrationBuilder.InsertData(
                table: "Cellars",
                columns: new[] { "CellarId", "Name", "NbDrawerMax", "UserId" },
                values: new object[,]
                {
                    { 1, "Cellar 1", 5, 1 },
                    { 2, "Cellar 2", 10, 2 },
                    { 3, "Cellar 3", 20, 3 }
                });

            migrationBuilder.InsertData(
                table: "Drawers",
                columns: new[] { "DrawerId", "CellarId", "Index", "NbBottleMax" },
                values: new object[,]
                {
                    { 1, 1, 1, 5 },
                    { 2, 1, 2, 5 },
                    { 3, 1, 3, 5 },
                    { 4, 2, 1, 5 },
                    { 5, 2, 2, 5 },
                    { 6, 2, 3, 5 }
                });

            migrationBuilder.InsertData(
                table: "Wines",
                columns: new[] { "WineId", "AppelationId", "Color", "DrawerId", "Name", "PictureName", "Year" },
                values: new object[,]
                {
                    { 1, 1, 0, 1, "20-1", "", 1960 },
                    { 2, 2, 0, 1, "20-2", "img/vin1.png", 1970 },
                    { 3, 3, 0, 2, "20-3", "", 1980 },
                    { 4, 4, 0, 2, "20-4", "", 1960 },
                    { 5, 5, 0, 3, "20-5", "", 1960 },
                    { 6, 6, 0, 3, "20-6", "", 1960 },
                    { 7, 12, 2, 4, "20-7", "", 1960 },
                    { 8, 13, 2, 4, "20-8", "", 1960 },
                    { 9, 14, 2, 5, "20-9", "", 1960 },
                    { 10, 12, 2, 5, "20-10", "", 1960 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cellars_UserId",
                table: "Cellars",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Drawers_CellarId",
                table: "Drawers",
                column: "CellarId");

            migrationBuilder.CreateIndex(
                name: "IX_Wines_AppelationId",
                table: "Wines",
                column: "AppelationId");

            migrationBuilder.CreateIndex(
                name: "IX_Wines_DrawerId",
                table: "Wines",
                column: "DrawerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Wines");

            migrationBuilder.DropTable(
                name: "Appelations");

            migrationBuilder.DropTable(
                name: "Drawers");

            migrationBuilder.DropTable(
                name: "Cellars");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
