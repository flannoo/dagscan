using DagScan.Application.Domain;
using DagScan.Application.Domain.ValueObjects;
using DagScan.Core.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DagScan.Application.Data;

public sealed class MetagraphSnapshotRewardEntityTypeConfiguration : IEntityTypeConfiguration<MetagraphSnapshotReward>
{
    public void Configure(EntityTypeBuilder<MetagraphSnapshotReward> builder)
    {
        builder.ToTable("MetagraphSnapshotRewards");

        builder.Property(x => x.Id)
            .ValueGeneratedNever()
            .HasConversion(
                id => id.Value,
                value => new MetagraphSnapshotRewardId(value)
            );

        builder.HasKey(x => x.Id);

        builder.Property(x => x.MetagraphId)
            .ValueGeneratedNever()
            .HasConversion(
                id => id.Value,
                value => new MetagraphId(value)
            );

        builder.HasOne<Metagraph>()
            .WithMany()
            .HasForeignKey(x => x.MetagraphId);

        builder.Property(x => x.WalletAddress)
            .HasMaxLength(DatabaseConstants.ColumnTypeLengths.WalletText)
            .IsRequired()
            .HasConversion(
                id => id.Value,
                value => new WalletAddress(value)
            );

        builder.Property(x => x.MetagraphAddress)
            .HasMaxLength(DatabaseConstants.ColumnTypeLengths.WalletText)
            .IsRequired()
            .HasConversion(
                id => id.Value,
                value => new MetagraphAddress(value)
            );

        builder.HasIndex(x => x.WalletAddress);
        builder.HasIndex(x => new { x.MetagraphId, x.SnapshotOrdinal, x.WalletAddress }).IsUnique();
    }
}
