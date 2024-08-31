using DagScan.Application.Domain;
using DagScan.Application.Domain.ValueObjects;
using DagScan.Core.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DagScan.Application.Data;

public sealed class HypergraphValidatorNodeParticipantEntityTypeConfiguration : IEntityTypeConfiguration<HypergraphValidatorNodeParticipant>
{
    public void Configure(EntityTypeBuilder<HypergraphValidatorNodeParticipant> builder)
    {
        builder.ToTable("HypergraphValidatorNodeParticipants");

        builder.Property(x => x.Id)
            .ValueGeneratedNever()
            .HasConversion(
                id => id.Value,
                value => new HypergraphValidatorNodeParticipantId(value)
            );

        builder.HasKey(x => x.Id);

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

        builder.Property(x => x.WalletId)
            .HasMaxLength(DatabaseConstants.ColumnTypeLengths.LongText)
            .IsRequired()
            .HasConversion(
                id => id.Value,
                value => new WalletId(value)
            );

        builder.HasIndex(x => new { x.HypergraphId, x.WalletId, x.SnapshotDate }).IsUnique();
    }
}
