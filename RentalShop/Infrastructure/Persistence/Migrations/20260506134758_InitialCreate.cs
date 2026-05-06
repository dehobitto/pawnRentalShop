using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace RentalShop.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PackageComponents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    ComponentType = table.Column<string>(type: "character varying(21)", maxLength: 21, nullable: false),
                    ParentPackageId = table.Column<int>(type: "integer", nullable: true),
                    Price = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PackageComponents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PackageComponents_PackageComponents_ParentPackageId",
                        column: x => x.ParentPackageId,
                        principalTable: "PackageComponents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RentalItems",
                columns: table => new
                {
                    Sku = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    BasePricePerDay = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    CurrentStateName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, defaultValue: "AvailableState"),
                    ItemType = table.Column<string>(type: "character varying(13)", maxLength: 13, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RentalItems", x => x.Sku);
                });

            migrationBuilder.InsertData(
                table: "RentalItems",
                columns: new[] { "Sku", "BasePricePerDay", "CurrentStateName", "ItemType", "Name" },
                values: new object[,]
                {
                    { "GEAR-001", 22m, "AvailableState", "Gear", "Camping Tent" },
                    { "GEAR-002", 8m, "AvailableState", "Gear", "Sleeping Bag" },
                    { "GEAR-003", 35m, "AvailableState", "Gear", "Kayak" },
                    { "TOOL-001", 12m, "AvailableState", "Tool", "Cordless Drill" },
                    { "TOOL-002", 18m, "AvailableState", "Tool", "Circular Saw" },
                    { "TOOL-003", 6m, "AvailableState", "Tool", "Hammer-set" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_PackageComponents_ParentPackageId",
                table: "PackageComponents",
                column: "ParentPackageId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PackageComponents");

            migrationBuilder.DropTable(
                name: "RentalItems");
        }
    }
}
