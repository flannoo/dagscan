using DagScan.Application.Domain;
using DagScan.Application.Domain.ValueObjects;
using DagScan.Core.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DagScan.Application.Data;

public sealed class HypergraphSnapshotMetricEntityTypeConfiguration : IEntityTypeConfiguration<HypergraphSnapshotMetric>
{
    public void Configure(EntityTypeBuilder<HypergraphSnapshotMetric> builder)
    {
        builder.ToTable("HypergraphSnapshotMetrics");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .UseIdentityColumn();

        builder.Property(x => x.MetagraphAddress)
            .HasMaxLength(DatabaseConstants.ColumnTypeLengths.WalletText)
            .IsRequired(false)
            .HasConversion(
                id => id!.Value,
                value => new WalletAddress(value)
            );
    }
}
