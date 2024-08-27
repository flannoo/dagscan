using DagScan.Application.Domain;
using DagScan.Application.Domain.ValueObjects;
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

        builder.HasIndex(v => new { v.WalletId, v.MetagraphType, v.MetagraphId }).IsUnique();
        builder.HasIndex(v => v.WalletId);
        builder.HasIndex(v => v.WalletAddress);

        builder.Property(v => v.MetagraphId)
            .ValueGeneratedNever()
            .HasConversion(
                id => id.Value,
                value => new MetagraphId(value)
            );

        builder.HasOne<Metagraph>()
            .WithMany()
            .HasForeignKey(v => v.MetagraphId);

        builder.Property(v => v.WalletAddress)
            .HasMaxLength(DatabaseConstants.ColumnTypeLengths.WalletText)
            .IsRequired()
            .HasConversion(
                id => id.Value,
                value => new WalletAddress(value)
            );

        builder.Property(v => v.WalletId)
            .HasMaxLength(DatabaseConstants.ColumnTypeLengths.LongText)
            .IsRequired()
            .HasConversion(
                id => id.Value,
                value => new WalletId(value)
            );

        builder.Property(v => v.IpAddress)
            .HasMaxLength(DatabaseConstants.ColumnTypeLengths.NormalText)
            .IsRequired();

        builder.Property(v => v.NodeStatus)
            .HasMaxLength(DatabaseConstants.ColumnTypeLengths.ShortText)
            .IsRequired();

        builder.Property(v => v.ServiceProvider)
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

        builder.OwnsOne(m => m.Coordinates, cb =>
        {
            cb.Property(c => c.Latitude).HasColumnName("Latitude");
            cb.Property(c => c.Longitude).HasColumnName("Longitude");
        });
    }
}
