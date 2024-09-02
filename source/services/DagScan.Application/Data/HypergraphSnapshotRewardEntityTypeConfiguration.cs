using DagScan.Application.Domain;
using DagScan.Application.Domain.ValueObjects;
using DagScan.Core.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DagScan.Application.Data;

public sealed class HypergraphSnapshotRewardEntityTypeConfiguration : IEntityTypeConfiguration<HypergraphSnapshotReward>
{
    public void Configure(EntityTypeBuilder<HypergraphSnapshotReward> builder)
    {
        builder.ToTable("HypergraphSnapshotRewards");

        builder.Property(x => x.Id)
            .ValueGeneratedNever()
            .HasConversion(
                id => id.Value,
                value => new HypergraphSnapshotRewardId(value)
            );

        builder.HasKey(x => x.Id);

        builder.Property(x => x.HypergraphId)
            .ValueGeneratedNever()
            .HasConversion(
                id => id.Value,
                value => new HypergraphId(value)
            );

        builder.HasOne<Hypergraph>()
            .WithMany()
            .HasForeignKey(h => h.HypergraphId);

        builder.Property(x => x.WalletAddress)
            .HasMaxLength(DatabaseConstants.ColumnTypeLengths.WalletText)
            .IsRequired()
            .HasConversion(
                id => id.Value,
                value => new WalletAddress(value)
            );

        builder.HasIndex(x => x.WalletAddress);
        builder.HasIndex(x => new { x.HypergraphId, x.RewardDate, x.WalletAddress }).IsUnique();
    }
}
