using DagScan.Application.Domain;
using DagScan.Application.Domain.ValueObjects;
using DagScan.Core.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DagScan.Application.Data;

public sealed class HypergraphBalanceEntityTypeConfiguration: IEntityTypeConfiguration<HypergraphBalance>
{
    public void Configure(EntityTypeBuilder<HypergraphBalance> builder)
    {
        builder.ToTable("HypergraphBalances");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedNever()
            .HasConversion(
                id => id.Value,
                value => new HypergraphBalanceId(value)
            );

        builder.Property(x => x.HypergraphId)
            .ValueGeneratedNever()
            .HasConversion(
                id => id.Value,
                value => new HypergraphId(value)
            );

        builder.Property(x => x.WalletAddress)
            .HasMaxLength(DatabaseConstants.ColumnTypeLengths.WalletText)
            .IsRequired()
            .HasConversion(
                id => id.Value,
                value => new WalletAddress(value)
            );

        builder.HasIndex(x => x.HypergraphId)
            .IncludeProperties(x => new { x.Balance, x.WalletAddress })
            .IsUnique(false)
            .IsCreatedOnline(true);
    }
}
