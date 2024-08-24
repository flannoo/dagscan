using DagScan.Application.Domain;
using DagScan.Core.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DagScan.Application.Data;

public sealed class HypergraphValidatorNodeEntityTypeConfiguration : IEntityTypeConfiguration<HypergraphValidatorNode>
{
    public void Configure(EntityTypeBuilder<HypergraphValidatorNode> builder)
    {
        builder.ToTable("HypergraphValidatorNodes");

        builder.Property(h => h.Id)
            .ValueGeneratedNever()
            .HasConversion(
                id => id.Value,
                value => new HypergraphValidatorNodeId(value)
            );

        builder.HasKey(h => h.Id);

        builder.HasIndex(h => new { h.WalletHash, h.WalletAddress }).IsUnique();

        builder.Property(h => h.WalletHash)
            .HasMaxLength(DatabaseConstants.ColumnTypeLengths.LongText)
            .IsRequired();

        builder.Property(h => h.WalletAddress)
            .HasMaxLength(DatabaseConstants.ColumnTypeLengths.NormalText)
            .IsRequired();

        builder.Property(h => h.Version)
            .HasMaxLength(DatabaseConstants.ColumnTypeLengths.ShortText)
            .IsRequired(false);

        builder.Property(h => h.State)
            .HasMaxLength(DatabaseConstants.ColumnTypeLengths.ShortText)
            .IsRequired(false);

        builder.Property(h => h.Provider)
            .HasMaxLength(DatabaseConstants.ColumnTypeLengths.MediumText)
            .IsRequired(false);

        builder.Property(h => h.Country)
            .HasMaxLength(DatabaseConstants.ColumnTypeLengths.NormalText)
            .IsRequired(false);

        builder.Property(h => h.City)
            .HasMaxLength(DatabaseConstants.ColumnTypeLengths.NormalText)
            .IsRequired(false);
    }
}
