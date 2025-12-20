using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ArtEva.Migrations
{
    /// <inheritdoc />
    public partial class ShopApprovalStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "CreatedAt" },
                values: new object[] { "dda8857f-f5b8-49d9-a734-026d2819e562", new DateTime(2025, 12, 15, 18, 24, 9, 650, DateTimeKind.Utc).AddTicks(6899) });

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "ConcurrencyStamp", "CreatedAt" },
                values: new object[] { "1067e2b1-9efc-42d9-9df2-c06c257c0c14", new DateTime(2025, 12, 15, 18, 24, 9, 650, DateTimeKind.Utc).AddTicks(6910) });

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "ConcurrencyStamp", "CreatedAt" },
                values: new object[] { "57dffefe-8cd4-4db8-8837-88bf12b9e410", new DateTime(2025, 12, 15, 18, 24, 9, 650, DateTimeKind.Utc).AddTicks(6917) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "CreatedAt" },
                values: new object[] { "98d82d24-f00c-48d5-981a-d56968c54eb6", new DateTime(2025, 12, 13, 16, 8, 6, 193, DateTimeKind.Utc).AddTicks(158) });

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "ConcurrencyStamp", "CreatedAt" },
                values: new object[] { "04937574-7500-4521-ba83-697dad3863e3", new DateTime(2025, 12, 13, 16, 8, 6, 193, DateTimeKind.Utc).AddTicks(170) });

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "ConcurrencyStamp", "CreatedAt" },
                values: new object[] { "12edd86e-7e1f-498d-b8d6-2105bf686fe2", new DateTime(2025, 12, 13, 16, 8, 6, 193, DateTimeKind.Utc).AddTicks(176) });
        }
    }
}
