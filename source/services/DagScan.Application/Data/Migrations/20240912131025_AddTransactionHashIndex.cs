using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DagScan.Application.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddTransactionHashIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_RewardTransactions_TransactionHash",
                table: "RewardTransactions",
                column: "TransactionHash",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RewardTransactions_WalletAddress",
                table: "RewardTransactions",
                column: "WalletAddress");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_RewardTransactions_TransactionHash",
                table: "RewardTransactions");

            migrationBuilder.DropIndex(
                name: "IX_RewardTransactions_WalletAddress",
                table: "RewardTransactions");
        }
    }
}
