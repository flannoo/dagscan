using System;
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
                    DataSyncEnabled = table.Column<bool>(type: "bit", nullable: false)
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
                name: "Metagraphs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    HypergraphId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Symbol = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    CompanyName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Website = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    DataSyncEnabled = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    MetagraphEndpoint = table.Column<string>(type: "nvarchar(max)", nullable: true)
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
                    WalletHash = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    WalletId = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    IpAddress = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    State = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    Version = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    Provider = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Country = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    City = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Latitude = table.Column<double>(type: "float", nullable: true),
                    Longitude = table.Column<double>(type: "float", nullable: true),
                    NodeOperatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
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
                name: "MetagraphValidatorNodes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MetagraphId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MetagraphType = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    WalletHash = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    WalletId = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    IpAddress = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    State = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    Version = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    Provider = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Country = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    City = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Latitude = table.Column<double>(type: "float", nullable: true),
                    Longitude = table.Column<double>(type: "float", nullable: true),
                    NodeOperatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
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

            migrationBuilder.CreateIndex(
                name: "IX_HypergraphValidatorNodes_HypergraphId",
                table: "HypergraphValidatorNodes",
                column: "HypergraphId");

            migrationBuilder.CreateIndex(
                name: "IX_HypergraphValidatorNodes_NodeOperatorId",
                table: "HypergraphValidatorNodes",
                column: "NodeOperatorId");

            migrationBuilder.CreateIndex(
                name: "IX_HypergraphValidatorNodes_WalletHash_WalletId",
                table: "HypergraphValidatorNodes",
                columns: new[] { "WalletHash", "WalletId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Metagraphs_HypergraphId",
                table: "Metagraphs",
                column: "HypergraphId");

            migrationBuilder.CreateIndex(
                name: "IX_MetagraphValidatorNodes_MetagraphId",
                table: "MetagraphValidatorNodes",
                column: "MetagraphId");

            migrationBuilder.CreateIndex(
                name: "IX_MetagraphValidatorNodes_NodeOperatorId",
                table: "MetagraphValidatorNodes",
                column: "NodeOperatorId");

            migrationBuilder.CreateIndex(
                name: "IX_MetagraphValidatorNodes_WalletHash_MetagraphType",
                table: "MetagraphValidatorNodes",
                columns: new[] { "WalletHash", "MetagraphType" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HypergraphValidatorNodes");

            migrationBuilder.DropTable(
                name: "MetagraphValidatorNodes");

            migrationBuilder.DropTable(
                name: "Metagraphs");

            migrationBuilder.DropTable(
                name: "NodeOperators");

            migrationBuilder.DropTable(
                name: "Hypergraphs");
        }
    }
}
