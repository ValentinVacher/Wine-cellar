using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Winecelar.Migrations
{
    /// <inheritdoc />
    public partial class Today : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Today",
                table: "Wines");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "Today",
                table: "Wines",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Wines",
                keyColumn: "WineId",
                keyValue: 1,
                column: "Today",
                value: new DateTime(2022, 12, 7, 10, 22, 3, 269, DateTimeKind.Local).AddTicks(9433));

            migrationBuilder.UpdateData(
                table: "Wines",
                keyColumn: "WineId",
                keyValue: 2,
                column: "Today",
                value: new DateTime(2022, 12, 7, 10, 22, 3, 269, DateTimeKind.Local).AddTicks(9482));

            migrationBuilder.UpdateData(
                table: "Wines",
                keyColumn: "WineId",
                keyValue: 3,
                column: "Today",
                value: new DateTime(2022, 12, 7, 10, 22, 3, 269, DateTimeKind.Local).AddTicks(9486));

            migrationBuilder.UpdateData(
                table: "Wines",
                keyColumn: "WineId",
                keyValue: 4,
                column: "Today",
                value: new DateTime(2022, 12, 7, 10, 22, 3, 269, DateTimeKind.Local).AddTicks(9490));

            migrationBuilder.UpdateData(
                table: "Wines",
                keyColumn: "WineId",
                keyValue: 5,
                column: "Today",
                value: new DateTime(2022, 12, 7, 10, 22, 3, 269, DateTimeKind.Local).AddTicks(9494));

            migrationBuilder.UpdateData(
                table: "Wines",
                keyColumn: "WineId",
                keyValue: 6,
                column: "Today",
                value: new DateTime(2022, 12, 7, 10, 22, 3, 269, DateTimeKind.Local).AddTicks(9557));

            migrationBuilder.UpdateData(
                table: "Wines",
                keyColumn: "WineId",
                keyValue: 7,
                column: "Today",
                value: new DateTime(2022, 12, 7, 10, 22, 3, 269, DateTimeKind.Local).AddTicks(9561));

            migrationBuilder.UpdateData(
                table: "Wines",
                keyColumn: "WineId",
                keyValue: 8,
                column: "Today",
                value: new DateTime(2022, 12, 7, 10, 22, 3, 269, DateTimeKind.Local).AddTicks(9565));

            migrationBuilder.UpdateData(
                table: "Wines",
                keyColumn: "WineId",
                keyValue: 9,
                column: "Today",
                value: new DateTime(2022, 12, 7, 10, 22, 3, 269, DateTimeKind.Local).AddTicks(9568));

            migrationBuilder.UpdateData(
                table: "Wines",
                keyColumn: "WineId",
                keyValue: 10,
                column: "Today",
                value: new DateTime(2022, 12, 7, 10, 22, 3, 269, DateTimeKind.Local).AddTicks(9572));
        }
    }
}
