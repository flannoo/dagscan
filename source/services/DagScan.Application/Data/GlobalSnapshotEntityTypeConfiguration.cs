using DagScan.Application.Domain;
using DagScan.Application.Domain.ValueObjects;
using DagScan.Core.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DagScan.Application.Data;

public sealed class GlobalSnapshotEntityTypeConfiguration : IEntityTypeConfiguration<GlobalSnapshot>
{
    public void Configure(EntityTypeBuilder<GlobalSnapshot> builder)
    {
        builder.ToTable("GlobalSnapshots");

        builder.Property(x => x.Id)
            .ValueGeneratedNever()
            .HasConversion(
                id => id.Value,
                value => new GlobalSnapshotId(value)
            );

        builder.HasKey(x => x.Id);

        builder.Property(x => x.HypergraphId)
            .ValueGeneratedNever()
            .HasConversion(
                id => id.Value,
                value => new HypergraphId(value)
            );

        builder.Property(x => x.Hash)
            .HasMaxLength(DatabaseConstants.ColumnTypeLengths.LongText)
            .IsRequired();

        builder.Property(x => x.MetagraphAddress)
            .HasMaxLength(DatabaseConstants.ColumnTypeLengths.WalletText)
            .IsRequired(false)
            .HasConversion(
                id => id!.Value,
                value => new WalletAddress(value)
            );

        builder.HasIndex(x => x.MetagraphAddress);

        builder.HasIndex(x => new { x.HypergraphId, x.Ordinal }).IsUnique();
    }
}
