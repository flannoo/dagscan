using DagScan.Application.Domain;
using DagScan.Application.Domain.ValueObjects;
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

        builder.Property(h => h.HypergraphId)
            .ValueGeneratedNever()
            .HasConversion(
                id => id.Value,
                value => new HypergraphId(value)
            );

        builder.HasOne<Hypergraph>()
            .WithMany()
            .HasForeignKey(h => h.HypergraphId);

        builder.HasKey(h => h.Id);

        builder.HasIndex(h => new { WalletHash = h.WalletAddress, h.WalletId }).IsUnique();

        builder.Property(h => h.WalletAddress)
            .HasMaxLength(DatabaseConstants.ColumnTypeLengths.WalletText)
            .IsRequired()
            .HasConversion(
                id => id.Value,
                value => new WalletAddress(value)
            );

        builder.Property(h => h.WalletId)
            .HasMaxLength(DatabaseConstants.ColumnTypeLengths.LongText)
            .IsRequired();

        builder.Property(h => h.IpAddress)
            .HasMaxLength(DatabaseConstants.ColumnTypeLengths.NormalText)
            .IsRequired();

        builder.Property(h => h.NodeStatus)
            .HasMaxLength(DatabaseConstants.ColumnTypeLengths.ShortText)
            .IsRequired();

        builder.Property(h => h.ServiceProvider)
            .HasMaxLength(DatabaseConstants.ColumnTypeLengths.MediumText)
            .IsRequired(false);

        builder.Property(h => h.Country)
            .HasMaxLength(DatabaseConstants.ColumnTypeLengths.NormalText)
            .IsRequired(false);

        builder.Property(h => h.City)
            .HasMaxLength(DatabaseConstants.ColumnTypeLengths.NormalText)
            .IsRequired(false);

        builder.OwnsOne(h => h.Coordinates, cb =>
        {
            cb.Property(c => c.Latitude).HasColumnName("Latitude");
            cb.Property(c => c.Longitude).HasColumnName("Longitude");
        });
    }
}
