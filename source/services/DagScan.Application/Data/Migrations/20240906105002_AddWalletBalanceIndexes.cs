using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DagScan.Application.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddWalletBalanceIndexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_MetagraphBalances_MetagraphId",
                table: "MetagraphBalances");

            migrationBuilder.DropIndex(
                name: "IX_HypergraphBalances_HypergraphId",
                table: "HypergraphBalances");

            migrationBuilder.CreateIndex(
                name: "IX_MetagraphBalances_MetagraphId",
                table: "MetagraphBalances",
                column: "MetagraphId")
                .Annotation("SqlServer:Include", new[] { "Balance", "MetagraphAddress", "WalletAddress" })
                .Annotation("SqlServer:Online", true);

            migrationBuilder.CreateIndex(
                name: "IX_HypergraphBalances_HypergraphId",
                table: "HypergraphBalances",
                column: "HypergraphId")
                .Annotation("SqlServer:Include", new[] { "Balance", "WalletAddress" })
                .Annotation("SqlServer:Online", true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_MetagraphBalances_MetagraphId",
                table: "MetagraphBalances");

            migrationBuilder.DropIndex(
                name: "IX_HypergraphBalances_HypergraphId",
                table: "HypergraphBalances");

            migrationBuilder.CreateIndex(
                name: "IX_MetagraphBalances_MetagraphId",
                table: "MetagraphBalances",
                column: "MetagraphId");

            migrationBuilder.CreateIndex(
                name: "IX_HypergraphBalances_HypergraphId",
                table: "HypergraphBalances",
                column: "HypergraphId");
        }
    }
}
