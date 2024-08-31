﻿using System.Reflection;
using DagScan.Application.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace DagScan.Application.Data;

public sealed class DagContext(DbContextOptions<DagContext> options) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        if (Database.IsSqlServer())
        {
            modelBuilder.UseCollation("SQL_Latin1_General_CP1_CS_AS");
        }

        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }

    public DbSet<Hypergraph> Hypergraphs => Set<Hypergraph>();
    public DbSet<HypergraphBalance> HypergraphBalances => Set<HypergraphBalance>();
    public DbSet<HypergraphValidatorNode> HypergraphValidatorNodes => Set<HypergraphValidatorNode>();
    public DbSet<GlobalSnapshot> GlobalSnapshots => Set<GlobalSnapshot>();
    public DbSet<GlobalSnapshotReward> GlobalSnapshotRewards => Set<GlobalSnapshotReward>();
    public DbSet<NodeOperator> ValidatorNodeOperators => Set<NodeOperator>();
    public DbSet<Metagraph> Metagraphs => Set<Metagraph>();
    public DbSet<MetagraphBalance> MetagraphBalances => Set<MetagraphBalance>();
    public DbSet<MetagraphValidatorNode> MetagraphValidatorNodes => Set<MetagraphValidatorNode>();
}

public sealed class ReadOnlyDagContext : DbContext
{
    public ReadOnlyDagContext(DbContextOptions<ReadOnlyDagContext> options) : base(options)
    {
        ChangeTracker.AutoDetectChangesEnabled = false;
        ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        if (Database.IsSqlServer())
        {
            modelBuilder.UseCollation("SQL_Latin1_General_CP1_CS_AS");
        }

        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }

    public DbSet<Hypergraph> Hypergraphs => Set<Hypergraph>();
    public DbSet<HypergraphBalance> HypergraphBalances => Set<HypergraphBalance>();
    public DbSet<HypergraphValidatorNode> HypergraphValidatorNodes => Set<HypergraphValidatorNode>();
    public DbSet<GlobalSnapshot> GlobalSnapshots => Set<GlobalSnapshot>();
    public DbSet<GlobalSnapshotReward> GlobalSnapshotRewards => Set<GlobalSnapshotReward>();
    public DbSet<NodeOperator> ValidatorNodeOperators => Set<NodeOperator>();
    public DbSet<Metagraph> Metagraphs => Set<Metagraph>();
    public DbSet<MetagraphBalance> MetagraphBalances => Set<MetagraphBalance>();
    public DbSet<MetagraphValidatorNode> MetagraphValidatorNodes => Set<MetagraphValidatorNode>();
}

public class DagContextFactory : IDesignTimeDbContextFactory<DagContext>
{
    public DagContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<DagContext>();
        optionsBuilder.UseSqlServer("Data Source=dagscan.db");

        return new DagContext(optionsBuilder.Options);
    }
}
