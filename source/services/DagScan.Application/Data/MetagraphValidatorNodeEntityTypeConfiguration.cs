using DagScan.Application.Domain;
using DagScan.Core.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DagScan.Application.Data;

public sealed class MetagraphValidatorNodeEntityTypeConfiguration : IEntityTypeConfiguration<MetagraphValidatorNode>
{
    public void Configure(EntityTypeBuilder<MetagraphValidatorNode> builder)
    {
        builder.ToTable("MetagraphValidatorNodes");

        builder.Property(v => v.Id)
            .ValueGeneratedNever()
            .HasConversion(
                id => id.Value,
                value => new MetagraphValidatorNodeId(value)
            );

        builder.HasKey(v => v.Id);

        builder.HasIndex(v => new { v.WalletHash, v.MetagraphType }).IsUnique();

        builder.Property(v => v.MetagraphId)
            .ValueGeneratedNever()
            .HasConversion(
                id => id.Value,
                value => new MetagraphId(value)
            );

        builder.HasOne<Metagraph>()
            .WithMany()
            .HasForeignKey(v => v.MetagraphId);

        builder.Property(v => v.WalletHash)
            .HasMaxLength(DatabaseConstants.ColumnTypeLengths.NormalText)
            .IsRequired();

        builder.Property(v => v.WalletId)
            .HasMaxLength(DatabaseConstants.ColumnTypeLengths.LongText)
            .IsRequired();

        builder.Property(v => v.IpAddress)
            .HasMaxLength(DatabaseConstants.ColumnTypeLengths.NormalText)
            .IsRequired();

        builder.Property(v => v.Version)
            .HasMaxLength(DatabaseConstants.ColumnTypeLengths.ShortText)
            .IsRequired(false);

        builder.Property(v => v.State)
            .HasMaxLength(DatabaseConstants.ColumnTypeLengths.ShortText)
            .IsRequired();

        builder.Property(v => v.Provider)
            .HasMaxLength(DatabaseConstants.ColumnTypeLengths.MediumText)
            .IsRequired(false);

        builder.Property(v => v.Country)
            .HasMaxLength(DatabaseConstants.ColumnTypeLengths.NormalText)
            .IsRequired(false);

        builder.Property(v => v.City)
            .HasMaxLength(DatabaseConstants.ColumnTypeLengths.NormalText)
            .IsRequired(false);

        builder.Property(v => v.MetagraphType)
            .HasMaxLength(DatabaseConstants.ColumnTypeLengths.ShortText)
            .HasConversion<string>()
            .IsRequired();
    }
}
