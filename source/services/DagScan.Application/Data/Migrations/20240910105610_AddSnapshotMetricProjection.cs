using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DagScan.Application.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddSnapshotMetricProjection : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HypergraphSnapshotMetrics",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SnapshotDate = table.Column<DateOnly>(type: "date", nullable: false),
                    MetagraphAddress = table.Column<string>(type: "nvarchar(51)", maxLength: 51, nullable: true),
                    IsTimeTriggered = table.Column<bool>(type: "bit", nullable: false),
                    TotalSnapshotCount = table.Column<long>(type: "bigint", nullable: false),
                    TotalSnapshotFeeAmount = table.Column<long>(type: "bigint", nullable: false),
                    TotalTransactionCount = table.Column<long>(type: "bigint", nullable: false),
                    TotalTransactionAmount = table.Column<long>(type: "bigint", nullable: false),
                    TotalTransactionFeeAmount = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HypergraphSnapshotMetrics", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HypergraphSnapshotMetrics");
        }
    }
}
