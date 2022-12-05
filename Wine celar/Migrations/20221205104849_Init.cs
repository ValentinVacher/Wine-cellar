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
                name: "Celars",
                columns: table => new
                {
                    CelarId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NbDrawerMax = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Celars", x => x.CelarId);
                    table.ForeignKey(
                        name: "FK_Celars_Users_UserId",
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
                    CelarId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Drawers", x => x.DrawerId);
                    table.ForeignKey(
                        name: "FK_Drawers_Celars_CelarId",
                        column: x => x.CelarId,
                        principalTable: "Celars",
                        principalColumn: "CelarId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Wines",
                columns: table => new
                {
                    WineId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Color = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Appelation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false),
                    Today = table.Column<DateTime>(type: "datetime2", nullable: false),
                    KeepMin = table.Column<int>(type: "int", nullable: false),
                    KeepMax = table.Column<int>(type: "int", nullable: false),
                    DrawerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wines", x => x.WineId);
                    table.ForeignKey(
                        name: "FK_Wines_Drawers_DrawerId",
                        column: x => x.DrawerId,
                        principalTable: "Drawers",
                        principalColumn: "DrawerId",
                        onDelete: ReferentialAction.Cascade);
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
                table: "Celars",
                columns: new[] { "CelarId", "Name", "NbDrawerMax", "UserId" },
                values: new object[,]
                {
                    { 1, "Celar 1", 5, 1 },
                    { 2, "Celar 2", 10, 2 },
                    { 3, "Celar 3", 20, 3 }
                });

            migrationBuilder.InsertData(
                table: "Drawers",
                columns: new[] { "DrawerId", "CelarId", "Index", "NbBottleMax" },
                values: new object[,]
                {
                    { 1, 1, 1, 0 },
                    { 2, 1, 2, 0 },
                    { 3, 1, 3, 0 },
                    { 4, 2, 1, 0 },
                    { 5, 2, 2, 0 },
                    { 6, 2, 3, 0 }
                });

            migrationBuilder.InsertData(
                table: "Wines",
                columns: new[] { "WineId", "Appelation", "Color", "DrawerId", "KeepMax", "KeepMin", "Name", "Today", "Year" },
                values: new object[,]
                {
                    { 1, "Appelation1", "Rosé", 1, 2002, 2000, "20-1", new DateTime(2022, 12, 5, 11, 48, 49, 595, DateTimeKind.Local).AddTicks(7003), 1960 },
                    { 2, "Appelation2", "Bleu", 1, 2002, 2001, "20-2", new DateTime(2022, 12, 5, 11, 48, 49, 595, DateTimeKind.Local).AddTicks(7035), 1970 },
                    { 3, "Appelation3", "Verre", 2, 2002, 2001, "20-3", new DateTime(2022, 12, 5, 11, 48, 49, 595, DateTimeKind.Local).AddTicks(7038), 1980 },
                    { 4, "Appelation4", "Rouge", 2, 2002, 2000, "20-4", new DateTime(2022, 12, 5, 11, 48, 49, 595, DateTimeKind.Local).AddTicks(7040), 1960 },
                    { 5, "Appelation5", "Jaune", 3, 2002, 2000, "20-5", new DateTime(2022, 12, 5, 11, 48, 49, 595, DateTimeKind.Local).AddTicks(7043), 1960 },
                    { 6, "Appelation6", "Blanc", 3, 2002, 2000, "20-6", new DateTime(2022, 12, 5, 11, 48, 49, 595, DateTimeKind.Local).AddTicks(7045), 1960 },
                    { 7, "Appelation7", "Rouge", 4, 2002, 2000, "20-7", new DateTime(2022, 12, 5, 11, 48, 49, 595, DateTimeKind.Local).AddTicks(7047), 1960 },
                    { 8, "Appelation8", "Violet", 4, 2002, 2000, "20-8", new DateTime(2022, 12, 5, 11, 48, 49, 595, DateTimeKind.Local).AddTicks(7049), 1960 },
                    { 9, "Appelation9", "Orange", 5, 2002, 2000, "20-9", new DateTime(2022, 12, 5, 11, 48, 49, 595, DateTimeKind.Local).AddTicks(7052), 1960 },
                    { 10, "Appelation10", "Violet", 5, 2002, 2000, "20-10", new DateTime(2022, 12, 5, 11, 48, 49, 595, DateTimeKind.Local).AddTicks(7054), 1960 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Celars_UserId",
                table: "Celars",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Drawers_CelarId",
                table: "Drawers",
                column: "CelarId");

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
                name: "Drawers");

            migrationBuilder.DropTable(
                name: "Celars");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
