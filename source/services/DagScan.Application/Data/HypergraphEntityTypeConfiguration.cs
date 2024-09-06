using DagScan.Application.Domain;
using DagScan.Application.Domain.ValueObjects;
using DagScan.Core.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DagScan.Application.Data;

public sealed class HypergraphEntityTypeConfiguration : IEntityTypeConfiguration<Hypergraph>
{
    public void Configure(EntityTypeBuilder<Hypergraph> builder)
    {
        builder.ToTable("Hypergraphs");

        builder.Property(h => h.Id)
            .ValueGeneratedNever()
            .HasConversion(
                id => id.Value,
                value => new HypergraphId(value)
            );

        builder.HasKey(h => h.Id);

        builder.Property(h => h.Name)
            .HasMaxLength(DatabaseConstants.ColumnTypeLengths.NormalText)
            .IsRequired();

        builder.Property(h => h.ApiBaseAddress)
            .HasMaxLength(DatabaseConstants.ColumnTypeLengths.NormalText)
            .IsRequired();

        builder.Property(h => h.BlockExplorerApiBaseAddress)
            .HasMaxLength(DatabaseConstants.ColumnTypeLengths.NormalText)
            .IsRequired(false);

        builder.OwnsMany<HypergraphBalance>(x => x.HypergraphBalances, balanceBuilder =>
        {
            balanceBuilder.ToTable("HypergraphBalances");

            balanceBuilder.HasKey(x => x.Id);

            balanceBuilder.Property(x => x.Id)
                .ValueGeneratedNever()
                .HasConversion(
                    id => id.Value,
                    value => new HypergraphBalanceId(value)
                );

            balanceBuilder.Property(x => x.HypergraphId)
                .ValueGeneratedNever()
                .HasConversion(
                    id => id.Value,
                    value => new HypergraphId(value)
                );

            balanceBuilder.Property(x => x.WalletAddress)
                .HasMaxLength(DatabaseConstants.ColumnTypeLengths.WalletText)
                .IsRequired()
                .HasConversion(
                    id => id.Value,
                    value => new WalletAddress(value)
                );

            balanceBuilder.HasIndex(x => x.HypergraphId)
                .IncludeProperties(x => new { x.Balance, x.WalletAddress })
                .IsUnique(false)
                .IsCreatedOnline(true);
        });

    }
}
