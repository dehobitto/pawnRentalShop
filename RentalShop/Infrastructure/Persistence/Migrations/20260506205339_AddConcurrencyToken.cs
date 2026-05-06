using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RentalShop.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddConcurrencyToken : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "Version",
                table: "RentalItems",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.UpdateData(
                table: "RentalItems",
                keyColumn: "Sku",
                keyValue: "GEAR-001",
                column: "Version",
                value: new Guid("bbbbbbbb-0002-0002-0002-000000000001"));

            migrationBuilder.UpdateData(
                table: "RentalItems",
                keyColumn: "Sku",
                keyValue: "GEAR-002",
                column: "Version",
                value: new Guid("bbbbbbbb-0002-0002-0002-000000000002"));

            migrationBuilder.UpdateData(
                table: "RentalItems",
                keyColumn: "Sku",
                keyValue: "GEAR-003",
                column: "Version",
                value: new Guid("bbbbbbbb-0002-0002-0002-000000000003"));

            migrationBuilder.UpdateData(
                table: "RentalItems",
                keyColumn: "Sku",
                keyValue: "TOOL-001",
                column: "Version",
                value: new Guid("aaaaaaaa-0001-0001-0001-000000000001"));

            migrationBuilder.UpdateData(
                table: "RentalItems",
                keyColumn: "Sku",
                keyValue: "TOOL-002",
                column: "Version",
                value: new Guid("aaaaaaaa-0001-0001-0001-000000000002"));

            migrationBuilder.UpdateData(
                table: "RentalItems",
                keyColumn: "Sku",
                keyValue: "TOOL-003",
                column: "Version",
                value: new Guid("aaaaaaaa-0001-0001-0001-000000000003"));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Version",
                table: "RentalItems");
        }
    }
}
