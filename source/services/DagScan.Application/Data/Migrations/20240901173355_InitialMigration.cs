﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DagScan.Application.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Hypergraphs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ApiBaseAddress = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    BlockExplorerApiBaseAddress = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DataSyncEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LastSnapshotSynced = table.Column<long>(type: "bigint", nullable: false),
                    StartSnapshotMetadataOrdinal = table.Column<long>(type: "bigint", nullable: false),
                    ConcurrencyVersion = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hypergraphs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NodeOperators",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Website = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NodeOperators", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HypergraphBalances",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    HypergraphId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WalletAddress = table.Column<string>(type: "nvarchar(51)", maxLength: 51, nullable: false),
                    Balance = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HypergraphBalances", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HypergraphBalances_Hypergraphs_HypergraphId",
                        column: x => x.HypergraphId,
                        principalTable: "Hypergraphs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HypergraphSnapshotRewards",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    HypergraphId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RewardDate = table.Column<DateOnly>(type: "date", nullable: false),
                    WalletAddress = table.Column<string>(type: "nvarchar(51)", maxLength: 51, nullable: false),
                    RewardAmount = table.Column<long>(type: "bigint", nullable: false),
                    LastReceivedUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HypergraphSnapshotRewards", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HypergraphSnapshotRewards_Hypergraphs_HypergraphId",
                        column: x => x.HypergraphId,
                        principalTable: "Hypergraphs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HypergraphSnapshots",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    HypergraphId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Ordinal = table.Column<long>(type: "bigint", nullable: false),
                    Blocks = table.Column<int>(type: "int", nullable: false),
                    Hash = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsTimeTriggeredSnapshot = table.Column<bool>(type: "bit", nullable: false),
                    FeeAmount = table.Column<long>(type: "bigint", nullable: true),
                    TransactionCount = table.Column<long>(type: "bigint", nullable: true),
                    TransactionAmount = table.Column<long>(type: "bigint", nullable: true),
                    TransactionFeeAmount = table.Column<long>(type: "bigint", nullable: true),
                    MetagraphAddress = table.Column<string>(type: "nvarchar(51)", maxLength: 51, nullable: true),
                    IsMetadataSynced = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HypergraphSnapshots", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HypergraphSnapshots_Hypergraphs_HypergraphId",
                        column: x => x.HypergraphId,
                        principalTable: "Hypergraphs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HypergraphValidatorNodeParticipants",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    HypergraphId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WalletAddress = table.Column<string>(type: "nvarchar(51)", maxLength: 51, nullable: false),
                    WalletId = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    SnapshotDate = table.Column<DateOnly>(type: "date", nullable: false),
                    SnapshotCount = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HypergraphValidatorNodeParticipants", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HypergraphValidatorNodeParticipants_Hypergraphs_HypergraphId",
                        column: x => x.HypergraphId,
                        principalTable: "Hypergraphs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Metagraphs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    HypergraphId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MetagraphAddress = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Symbol = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    FeeAddress = table.Column<string>(type: "nvarchar(51)", maxLength: 51, nullable: true),
                    CompanyName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Website = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    DataSyncEnabled = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    LastSnapshotSynced = table.Column<long>(type: "bigint", nullable: false),
                    ConcurrencyVersion = table.Column<int>(type: "int", nullable: false),
                    MetagraphEndpoints = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Metagraphs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Metagraphs_Hypergraphs_HypergraphId",
                        column: x => x.HypergraphId,
                        principalTable: "Hypergraphs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HypergraphValidatorNodes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    HypergraphId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WalletAddress = table.Column<string>(type: "nvarchar(51)", maxLength: 51, nullable: false),
                    WalletId = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    IpAddress = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    NodeStatus = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    IsInConsensus = table.Column<bool>(type: "bit", nullable: false),
                    LastModifiedUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ServiceProvider = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Country = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    City = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Latitude = table.Column<double>(type: "float", nullable: true),
                    Longitude = table.Column<double>(type: "float", nullable: true),
                    NodeOperatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ConcurrencyVersion = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HypergraphValidatorNodes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HypergraphValidatorNodes_Hypergraphs_HypergraphId",
                        column: x => x.HypergraphId,
                        principalTable: "Hypergraphs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HypergraphValidatorNodes_NodeOperators_NodeOperatorId",
                        column: x => x.NodeOperatorId,
                        principalTable: "NodeOperators",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "MetagraphBalances",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MetagraphId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MetagraphAddress = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    WalletAddress = table.Column<string>(type: "nvarchar(51)", maxLength: 51, nullable: false),
                    Balance = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MetagraphBalances", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MetagraphBalances_Metagraphs_MetagraphId",
                        column: x => x.MetagraphId,
                        principalTable: "Metagraphs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MetagraphSnapshotRewards",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MetagraphId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MetagraphAddress = table.Column<string>(type: "nvarchar(51)", maxLength: 51, nullable: false),
                    RewardDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SnapshotOrdinal = table.Column<long>(type: "bigint", nullable: false),
                    WalletAddress = table.Column<string>(type: "nvarchar(51)", maxLength: 51, nullable: false),
                    RewardAmount = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MetagraphSnapshotRewards", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MetagraphSnapshotRewards_Metagraphs_MetagraphId",
                        column: x => x.MetagraphId,
                        principalTable: "Metagraphs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MetagraphValidatorNodes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MetagraphId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MetagraphType = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    WalletAddress = table.Column<string>(type: "nvarchar(51)", maxLength: 51, nullable: false),
                    WalletId = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    IpAddress = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    NodeStatus = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    LastModifiedUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ServiceProvider = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Country = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    City = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Latitude = table.Column<double>(type: "float", nullable: true),
                    Longitude = table.Column<double>(type: "float", nullable: true),
                    NodeOperatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ConcurrencyVersion = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MetagraphValidatorNodes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MetagraphValidatorNodes_Metagraphs_MetagraphId",
                        column: x => x.MetagraphId,
                        principalTable: "Metagraphs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MetagraphValidatorNodes_NodeOperators_NodeOperatorId",
                        column: x => x.NodeOperatorId,
                        principalTable: "NodeOperators",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RewardTransactionConfigs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RewardCategory = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    MetagraphId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MetagraphAddress = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    FromWalletAddress = table.Column<string>(type: "nvarchar(51)", maxLength: 51, nullable: false),
                    ToWalletAddress = table.Column<string>(type: "nvarchar(51)", maxLength: 51, nullable: true),
                    LastProcessedHash = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    IsEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LastProcessedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RewardTransactionConfigs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RewardTransactionConfigs_Metagraphs_MetagraphId",
                        column: x => x.MetagraphId,
                        principalTable: "Metagraphs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RewardTransactions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RewardTransactionConfigId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MetagraphId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MetagraphAddress = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    WalletAddress = table.Column<string>(type: "nvarchar(51)", maxLength: 51, nullable: false),
                    RewardCategory = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    TransactionHash = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Amount = table.Column<long>(type: "bigint", nullable: false),
                    TransactionDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RewardTransactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RewardTransactions_Metagraphs_MetagraphId",
                        column: x => x.MetagraphId,
                        principalTable: "Metagraphs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HypergraphBalances_HypergraphId",
                table: "HypergraphBalances",
                column: "HypergraphId");

            migrationBuilder.CreateIndex(
                name: "IX_HypergraphSnapshotRewards_HypergraphId_RewardDate_WalletAddress",
                table: "HypergraphSnapshotRewards",
                columns: new[] { "HypergraphId", "RewardDate", "WalletAddress" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_HypergraphSnapshotRewards_WalletAddress",
                table: "HypergraphSnapshotRewards",
                column: "WalletAddress");

            migrationBuilder.CreateIndex(
                name: "IX_HypergraphSnapshots_HypergraphId_Ordinal",
                table: "HypergraphSnapshots",
                columns: new[] { "HypergraphId", "Ordinal" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_HypergraphSnapshots_MetagraphAddress",
                table: "HypergraphSnapshots",
                column: "MetagraphAddress");

            migrationBuilder.CreateIndex(
                name: "IX_HypergraphValidatorNodeParticipants_HypergraphId_WalletId_SnapshotDate",
                table: "HypergraphValidatorNodeParticipants",
                columns: new[] { "HypergraphId", "WalletId", "SnapshotDate" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_HypergraphValidatorNodes_HypergraphId",
                table: "HypergraphValidatorNodes",
                column: "HypergraphId");

            migrationBuilder.CreateIndex(
                name: "IX_HypergraphValidatorNodes_NodeOperatorId",
                table: "HypergraphValidatorNodes",
                column: "NodeOperatorId");

            migrationBuilder.CreateIndex(
                name: "IX_HypergraphValidatorNodes_WalletAddress",
                table: "HypergraphValidatorNodes",
                column: "WalletAddress");

            migrationBuilder.CreateIndex(
                name: "IX_HypergraphValidatorNodes_WalletId",
                table: "HypergraphValidatorNodes",
                column: "WalletId");

            migrationBuilder.CreateIndex(
                name: "IX_HypergraphValidatorNodes_WalletId_HypergraphId",
                table: "HypergraphValidatorNodes",
                columns: new[] { "WalletId", "HypergraphId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MetagraphBalances_MetagraphId",
                table: "MetagraphBalances",
                column: "MetagraphId");

            migrationBuilder.CreateIndex(
                name: "IX_Metagraphs_HypergraphId_MetagraphAddress",
                table: "Metagraphs",
                columns: new[] { "HypergraphId", "MetagraphAddress" },
                unique: true,
                filter: "[MetagraphAddress] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_MetagraphSnapshotRewards_MetagraphId_SnapshotOrdinal_WalletAddress",
                table: "MetagraphSnapshotRewards",
                columns: new[] { "MetagraphId", "SnapshotOrdinal", "WalletAddress" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MetagraphSnapshotRewards_WalletAddress",
                table: "MetagraphSnapshotRewards",
                column: "WalletAddress");

            migrationBuilder.CreateIndex(
                name: "IX_MetagraphValidatorNodes_MetagraphId",
                table: "MetagraphValidatorNodes",
                column: "MetagraphId");

            migrationBuilder.CreateIndex(
                name: "IX_MetagraphValidatorNodes_NodeOperatorId",
                table: "MetagraphValidatorNodes",
                column: "NodeOperatorId");

            migrationBuilder.CreateIndex(
                name: "IX_MetagraphValidatorNodes_WalletAddress",
                table: "MetagraphValidatorNodes",
                column: "WalletAddress");

            migrationBuilder.CreateIndex(
                name: "IX_MetagraphValidatorNodes_WalletId",
                table: "MetagraphValidatorNodes",
                column: "WalletId");

            migrationBuilder.CreateIndex(
                name: "IX_MetagraphValidatorNodes_WalletId_MetagraphType_MetagraphId",
                table: "MetagraphValidatorNodes",
                columns: new[] { "WalletId", "MetagraphType", "MetagraphId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RewardTransactionConfigs_MetagraphId",
                table: "RewardTransactionConfigs",
                column: "MetagraphId");

            migrationBuilder.CreateIndex(
                name: "IX_RewardTransactions_MetagraphId",
                table: "RewardTransactions",
                column: "MetagraphId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HypergraphBalances");

            migrationBuilder.DropTable(
                name: "HypergraphSnapshotRewards");

            migrationBuilder.DropTable(
                name: "HypergraphSnapshots");

            migrationBuilder.DropTable(
                name: "HypergraphValidatorNodeParticipants");

            migrationBuilder.DropTable(
                name: "HypergraphValidatorNodes");

            migrationBuilder.DropTable(
                name: "MetagraphBalances");

            migrationBuilder.DropTable(
                name: "MetagraphSnapshotRewards");

            migrationBuilder.DropTable(
                name: "MetagraphValidatorNodes");

            migrationBuilder.DropTable(
                name: "RewardTransactionConfigs");

            migrationBuilder.DropTable(
                name: "RewardTransactions");

            migrationBuilder.DropTable(
                name: "NodeOperators");

            migrationBuilder.DropTable(
                name: "Metagraphs");

            migrationBuilder.DropTable(
                name: "Hypergraphs");
        }
    }
}
