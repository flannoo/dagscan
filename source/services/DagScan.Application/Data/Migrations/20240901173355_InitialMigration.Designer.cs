﻿// <auto-generated />
using System;
using DagScan.Application.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DagScan.Application.Data.Migrations
{
    [DbContext(typeof(DagContext))]
    [Migration("20240901173355_InitialMigration")]
    partial class InitialMigration
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseCollation("SQL_Latin1_General_CP1_CS_AS")
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("DagScan.Application.Domain.Hypergraph", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ApiBaseAddress")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("BlockExplorerApiBaseAddress")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("ConcurrencyVersion")
                        .HasColumnType("int");

                    b.Property<bool>("DataSyncEnabled")
                        .HasColumnType("bit");

                    b.Property<long>("LastSnapshotSynced")
                        .HasColumnType("bigint");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<long>("StartSnapshotMetadataOrdinal")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.ToTable("Hypergraphs", (string)null);
                });

            modelBuilder.Entity("DagScan.Application.Domain.HypergraphSnapshot", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Blocks")
                        .HasColumnType("int");

                    b.Property<long?>("FeeAmount")
                        .HasColumnType("bigint");

                    b.Property<string>("Hash")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)");

                    b.Property<Guid>("HypergraphId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("IsMetadataSynced")
                        .HasColumnType("bit");

                    b.Property<bool>("IsTimeTriggeredSnapshot")
                        .HasColumnType("bit");

                    b.Property<string>("MetagraphAddress")
                        .HasMaxLength(51)
                        .HasColumnType("nvarchar(51)");

                    b.Property<long>("Ordinal")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("Timestamp")
                        .HasColumnType("datetime2");

                    b.Property<long?>("TransactionAmount")
                        .HasColumnType("bigint");

                    b.Property<long?>("TransactionCount")
                        .HasColumnType("bigint");

                    b.Property<long?>("TransactionFeeAmount")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("MetagraphAddress");

                    b.HasIndex("HypergraphId", "Ordinal")
                        .IsUnique();

                    b.ToTable("HypergraphSnapshots", (string)null);
                });

            modelBuilder.Entity("DagScan.Application.Domain.HypergraphSnapshotReward", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("HypergraphId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("LastReceivedUtc")
                        .HasColumnType("datetime2");

                    b.Property<long>("RewardAmount")
                        .HasColumnType("bigint");

                    b.Property<DateOnly>("RewardDate")
                        .HasColumnType("date");

                    b.Property<string>("WalletAddress")
                        .IsRequired()
                        .HasMaxLength(51)
                        .HasColumnType("nvarchar(51)");

                    b.HasKey("Id");

                    b.HasIndex("WalletAddress");

                    b.HasIndex("HypergraphId", "RewardDate", "WalletAddress")
                        .IsUnique();

                    b.ToTable("HypergraphSnapshotRewards", (string)null);
                });

            modelBuilder.Entity("DagScan.Application.Domain.HypergraphValidatorNode", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("City")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("ConcurrencyVersion")
                        .HasColumnType("int");

                    b.Property<string>("Country")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<Guid>("HypergraphId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("IpAddress")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<bool>("IsInConsensus")
                        .HasColumnType("bit");

                    b.Property<DateTime>("LastModifiedUtc")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("NodeOperatorId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("NodeStatus")
                        .IsRequired()
                        .HasMaxLength(25)
                        .HasColumnType("nvarchar(25)");

                    b.Property<string>("ServiceProvider")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("WalletAddress")
                        .IsRequired()
                        .HasMaxLength(51)
                        .HasColumnType("nvarchar(51)");

                    b.Property<string>("WalletId")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)");

                    b.HasKey("Id");

                    b.HasIndex("HypergraphId");

                    b.HasIndex("NodeOperatorId");

                    b.HasIndex("WalletAddress");

                    b.HasIndex("WalletId");

                    b.HasIndex("WalletId", "HypergraphId")
                        .IsUnique();

                    b.ToTable("HypergraphValidatorNodes", (string)null);
                });

            modelBuilder.Entity("DagScan.Application.Domain.HypergraphValidatorNodeParticipant", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("HypergraphId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<long>("SnapshotCount")
                        .HasColumnType("bigint");

                    b.Property<DateOnly>("SnapshotDate")
                        .HasColumnType("date");

                    b.Property<string>("WalletAddress")
                        .IsRequired()
                        .HasMaxLength(51)
                        .HasColumnType("nvarchar(51)");

                    b.Property<string>("WalletId")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)");

                    b.HasKey("Id");

                    b.HasIndex("HypergraphId", "WalletId", "SnapshotDate")
                        .IsUnique();

                    b.ToTable("HypergraphValidatorNodeParticipants", (string)null);
                });

            modelBuilder.Entity("DagScan.Application.Domain.Metagraph", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("CompanyName")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("ConcurrencyVersion")
                        .HasColumnType("int");

                    b.Property<bool>("DataSyncEnabled")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false);

                    b.Property<string>("Description")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<string>("FeeAddress")
                        .HasMaxLength(51)
                        .HasColumnType("nvarchar(51)");

                    b.Property<Guid>("HypergraphId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<long>("LastSnapshotSynced")
                        .HasColumnType("bigint");

                    b.Property<string>("MetagraphAddress")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Symbol")
                        .IsRequired()
                        .HasMaxLength(15)
                        .HasColumnType("nvarchar(15)");

                    b.Property<string>("Website")
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)");

                    b.HasKey("Id");

                    b.HasIndex("HypergraphId", "MetagraphAddress")
                        .IsUnique()
                        .HasFilter("[MetagraphAddress] IS NOT NULL");

                    b.ToTable("Metagraphs", (string)null);
                });

            modelBuilder.Entity("DagScan.Application.Domain.MetagraphSnapshotReward", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("MetagraphAddress")
                        .IsRequired()
                        .HasMaxLength(51)
                        .HasColumnType("nvarchar(51)");

                    b.Property<Guid>("MetagraphId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<long>("RewardAmount")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("RewardDate")
                        .HasColumnType("datetime2");

                    b.Property<long>("SnapshotOrdinal")
                        .HasColumnType("bigint");

                    b.Property<string>("WalletAddress")
                        .IsRequired()
                        .HasMaxLength(51)
                        .HasColumnType("nvarchar(51)");

                    b.HasKey("Id");

                    b.HasIndex("WalletAddress");

                    b.HasIndex("MetagraphId", "SnapshotOrdinal", "WalletAddress")
                        .IsUnique();

                    b.ToTable("MetagraphSnapshotRewards", (string)null);
                });

            modelBuilder.Entity("DagScan.Application.Domain.MetagraphValidatorNode", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("City")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("ConcurrencyVersion")
                        .HasColumnType("int");

                    b.Property<string>("Country")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("IpAddress")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<DateTime>("LastModifiedUtc")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("MetagraphId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("MetagraphType")
                        .IsRequired()
                        .HasMaxLength(25)
                        .HasColumnType("nvarchar(25)");

                    b.Property<Guid?>("NodeOperatorId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("NodeStatus")
                        .IsRequired()
                        .HasMaxLength(25)
                        .HasColumnType("nvarchar(25)");

                    b.Property<string>("ServiceProvider")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("WalletAddress")
                        .IsRequired()
                        .HasMaxLength(51)
                        .HasColumnType("nvarchar(51)");

                    b.Property<string>("WalletId")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)");

                    b.HasKey("Id");

                    b.HasIndex("MetagraphId");

                    b.HasIndex("NodeOperatorId");

                    b.HasIndex("WalletAddress");

                    b.HasIndex("WalletId");

                    b.HasIndex("WalletId", "MetagraphType", "MetagraphId")
                        .IsUnique();

                    b.ToTable("MetagraphValidatorNodes", (string)null);
                });

            modelBuilder.Entity("DagScan.Application.Domain.NodeOperator", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Description")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Website")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id");

                    b.ToTable("NodeOperators", (string)null);
                });

            modelBuilder.Entity("DagScan.Application.Domain.RewardTransaction", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<long>("Amount")
                        .HasColumnType("bigint");

                    b.Property<string>("MetagraphAddress")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<Guid>("MetagraphId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("RewardCategory")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<Guid>("RewardTransactionConfigId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("TransactionDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("TransactionHash")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)");

                    b.Property<string>("WalletAddress")
                        .IsRequired()
                        .HasMaxLength(51)
                        .HasColumnType("nvarchar(51)");

                    b.HasKey("Id");

                    b.HasIndex("MetagraphId");

                    b.ToTable("RewardTransactions", (string)null);
                });

            modelBuilder.Entity("DagScan.Application.Domain.RewardTransactionConfig", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("FromWalletAddress")
                        .IsRequired()
                        .HasMaxLength(51)
                        .HasColumnType("nvarchar(51)");

                    b.Property<bool>("IsEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTime>("LastProcessedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("LastProcessedHash")
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)");

                    b.Property<string>("MetagraphAddress")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<Guid>("MetagraphId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("RewardCategory")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("ToWalletAddress")
                        .HasMaxLength(51)
                        .HasColumnType("nvarchar(51)");

                    b.HasKey("Id");

                    b.HasIndex("MetagraphId");

                    b.ToTable("RewardTransactionConfigs", (string)null);
                });

            modelBuilder.Entity("DagScan.Application.Domain.Hypergraph", b =>
                {
                    b.OwnsMany("DagScan.Application.Domain.HypergraphBalance", "HypergraphBalances", b1 =>
                        {
                            b1.Property<Guid>("Id")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<long>("Balance")
                                .HasColumnType("bigint");

                            b1.Property<Guid>("HypergraphId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<string>("WalletAddress")
                                .IsRequired()
                                .HasMaxLength(51)
                                .HasColumnType("nvarchar(51)");

                            b1.HasKey("Id");

                            b1.HasIndex("HypergraphId");

                            b1.ToTable("HypergraphBalances", (string)null);

                            b1.WithOwner()
                                .HasForeignKey("HypergraphId");
                        });

                    b.Navigation("HypergraphBalances");
                });

            modelBuilder.Entity("DagScan.Application.Domain.HypergraphSnapshot", b =>
                {
                    b.HasOne("DagScan.Application.Domain.Hypergraph", null)
                        .WithMany()
                        .HasForeignKey("HypergraphId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("DagScan.Application.Domain.HypergraphSnapshotReward", b =>
                {
                    b.HasOne("DagScan.Application.Domain.Hypergraph", null)
                        .WithMany()
                        .HasForeignKey("HypergraphId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("DagScan.Application.Domain.HypergraphValidatorNode", b =>
                {
                    b.HasOne("DagScan.Application.Domain.Hypergraph", null)
                        .WithMany()
                        .HasForeignKey("HypergraphId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DagScan.Application.Domain.NodeOperator", "NodeOperator")
                        .WithMany()
                        .HasForeignKey("NodeOperatorId");

                    b.OwnsOne("DagScan.Application.Domain.ValueObjects.Coordinate", "Coordinates", b1 =>
                        {
                            b1.Property<Guid>("HypergraphValidatorNodeId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<double>("Latitude")
                                .HasColumnType("float")
                                .HasColumnName("Latitude");

                            b1.Property<double>("Longitude")
                                .HasColumnType("float")
                                .HasColumnName("Longitude");

                            b1.HasKey("HypergraphValidatorNodeId");

                            b1.ToTable("HypergraphValidatorNodes");

                            b1.WithOwner()
                                .HasForeignKey("HypergraphValidatorNodeId");
                        });

                    b.Navigation("Coordinates");

                    b.Navigation("NodeOperator");
                });

            modelBuilder.Entity("DagScan.Application.Domain.HypergraphValidatorNodeParticipant", b =>
                {
                    b.HasOne("DagScan.Application.Domain.Hypergraph", null)
                        .WithMany()
                        .HasForeignKey("HypergraphId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("DagScan.Application.Domain.Metagraph", b =>
                {
                    b.HasOne("DagScan.Application.Domain.Hypergraph", null)
                        .WithMany()
                        .HasForeignKey("HypergraphId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.OwnsMany("DagScan.Application.Domain.MetagraphBalance", "MetagraphBalances", b1 =>
                        {
                            b1.Property<Guid>("Id")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<long>("Balance")
                                .HasColumnType("bigint");

                            b1.Property<string>("MetagraphAddress")
                                .IsRequired()
                                .HasMaxLength(50)
                                .HasColumnType("nvarchar(50)");

                            b1.Property<Guid>("MetagraphId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<string>("WalletAddress")
                                .IsRequired()
                                .HasMaxLength(51)
                                .HasColumnType("nvarchar(51)");

                            b1.HasKey("Id");

                            b1.HasIndex("MetagraphId");

                            b1.ToTable("MetagraphBalances", (string)null);

                            b1.WithOwner()
                                .HasForeignKey("MetagraphId");
                        });

                    b.OwnsMany("DagScan.Application.Domain.ValueObjects.MetagraphEndpoint", "MetagraphEndpoints", b1 =>
                        {
                            b1.Property<Guid>("MetagraphId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<int>("Id")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("int");

                            b1.Property<string>("ApiBaseAddress")
                                .IsRequired()
                                .HasMaxLength(100)
                                .HasColumnType("nvarchar(100)");

                            b1.Property<string>("MetagraphType")
                                .IsRequired()
                                .HasMaxLength(25)
                                .HasColumnType("nvarchar(25)");

                            b1.HasKey("MetagraphId", "Id");

                            b1.ToTable("Metagraphs");

                            b1.ToJson("MetagraphEndpoints");

                            b1.WithOwner()
                                .HasForeignKey("MetagraphId");
                        });

                    b.Navigation("MetagraphBalances");

                    b.Navigation("MetagraphEndpoints");
                });

            modelBuilder.Entity("DagScan.Application.Domain.MetagraphSnapshotReward", b =>
                {
                    b.HasOne("DagScan.Application.Domain.Metagraph", null)
                        .WithMany()
                        .HasForeignKey("MetagraphId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("DagScan.Application.Domain.MetagraphValidatorNode", b =>
                {
                    b.HasOne("DagScan.Application.Domain.Metagraph", null)
                        .WithMany()
                        .HasForeignKey("MetagraphId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DagScan.Application.Domain.NodeOperator", "NodeOperator")
                        .WithMany()
                        .HasForeignKey("NodeOperatorId");

                    b.OwnsOne("DagScan.Application.Domain.ValueObjects.Coordinate", "Coordinates", b1 =>
                        {
                            b1.Property<Guid>("MetagraphValidatorNodeId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<double>("Latitude")
                                .HasColumnType("float")
                                .HasColumnName("Latitude");

                            b1.Property<double>("Longitude")
                                .HasColumnType("float")
                                .HasColumnName("Longitude");

                            b1.HasKey("MetagraphValidatorNodeId");

                            b1.ToTable("MetagraphValidatorNodes");

                            b1.WithOwner()
                                .HasForeignKey("MetagraphValidatorNodeId");
                        });

                    b.Navigation("Coordinates");

                    b.Navigation("NodeOperator");
                });

            modelBuilder.Entity("DagScan.Application.Domain.RewardTransaction", b =>
                {
                    b.HasOne("DagScan.Application.Domain.Metagraph", null)
                        .WithMany()
                        .HasForeignKey("MetagraphId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("DagScan.Application.Domain.RewardTransactionConfig", b =>
                {
                    b.HasOne("DagScan.Application.Domain.Metagraph", null)
                        .WithMany()
                        .HasForeignKey("MetagraphId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
