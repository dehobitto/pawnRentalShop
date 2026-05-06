using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RentalShop.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddExpectedReturnDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ExpectedReturnDate",
                table: "RentalItems",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "RentalItems",
                keyColumn: "Sku",
                keyValue: "GEAR-001",
                column: "ExpectedReturnDate",
                value: null);

            migrationBuilder.UpdateData(
                table: "RentalItems",
                keyColumn: "Sku",
                keyValue: "GEAR-002",
                column: "ExpectedReturnDate",
                value: null);

            migrationBuilder.UpdateData(
                table: "RentalItems",
                keyColumn: "Sku",
                keyValue: "GEAR-003",
                column: "ExpectedReturnDate",
                value: null);

            migrationBuilder.UpdateData(
                table: "RentalItems",
                keyColumn: "Sku",
                keyValue: "TOOL-001",
                column: "ExpectedReturnDate",
                value: null);

            migrationBuilder.UpdateData(
                table: "RentalItems",
                keyColumn: "Sku",
                keyValue: "TOOL-002",
                column: "ExpectedReturnDate",
                value: null);

            migrationBuilder.UpdateData(
                table: "RentalItems",
                keyColumn: "Sku",
                keyValue: "TOOL-003",
                column: "ExpectedReturnDate",
                value: null);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExpectedReturnDate",
                table: "RentalItems");
        }
    }
}
