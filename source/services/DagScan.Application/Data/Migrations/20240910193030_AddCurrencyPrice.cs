using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DagScan.Application.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddCurrencyPrice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CoingeckoId",
                table: "Metagraphs",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CurrencyPrices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateOnly>(type: "date", nullable: false),
                    MetagraphAddress = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Price = table.Column<decimal>(type: "decimal(30,18)", precision: 30, scale: 18, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CurrencyPrices", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CurrencyPrices");

            migrationBuilder.DropColumn(
                name: "CoingeckoId",
                table: "Metagraphs");
        }
    }
}
