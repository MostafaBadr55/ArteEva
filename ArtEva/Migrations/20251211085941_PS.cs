using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ArtEva.Migrations
{
    /// <inheritdoc />
    public partial class PS : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "CreatedAt" },
                values: new object[] { "b0bbfba2-4801-449a-abe3-31e2a3abb086", new DateTime(2025, 12, 11, 8, 59, 40, 913, DateTimeKind.Utc).AddTicks(3367) });

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "ConcurrencyStamp", "CreatedAt" },
                values: new object[] { "2e7e2a46-0bc1-4fd3-9f1a-e36d01d91cf0", new DateTime(2025, 12, 11, 8, 59, 40, 913, DateTimeKind.Utc).AddTicks(3387) });

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "ConcurrencyStamp", "CreatedAt" },
                values: new object[] { "617e1ec1-4a82-47dd-b01a-b30cd6919254", new DateTime(2025, 12, 11, 8, 59, 40, 913, DateTimeKind.Utc).AddTicks(3393) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Products");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "CreatedAt" },
                values: new object[] { "76aae604-f790-408f-aec1-3adeded77763", new DateTime(2025, 12, 11, 8, 55, 27, 515, DateTimeKind.Utc).AddTicks(208) });

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "ConcurrencyStamp", "CreatedAt" },
                values: new object[] { "1aec4f1a-9148-49b8-8984-830a8edec626", new DateTime(2025, 12, 11, 8, 55, 27, 515, DateTimeKind.Utc).AddTicks(218) });

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "ConcurrencyStamp", "CreatedAt" },
                values: new object[] { "cfa2c732-58eb-4485-8921-70df7ef1ad85", new DateTime(2025, 12, 11, 8, 55, 27, 515, DateTimeKind.Utc).AddTicks(243) });
        }
    }
}
