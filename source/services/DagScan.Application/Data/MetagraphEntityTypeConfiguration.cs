using DagScan.Application.Domain;
using DagScan.Application.Domain.ValueObjects;
using DagScan.Core.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DagScan.Application.Data;

public sealed class MetagraphEntityTypeConfiguration : IEntityTypeConfiguration<Metagraph>
{
    public void Configure(EntityTypeBuilder<Metagraph> builder)
    {
        builder.ToTable("Metagraphs");

        builder.Property(m => m.Id)
            .ValueGeneratedNever()
            .HasMaxLength(DatabaseConstants.ColumnTypeLengths.NormalText)
            .HasConversion(
                id => id.Value,
                value => new MetagraphId(value)
            );

        builder.HasKey(m => m.Id);

        builder.Property(m => m.HypergraphId)
            .ValueGeneratedNever()
            .HasConversion(
                id => id.Value,
                value => new HypergraphId(value)
            );

        builder.HasOne<Hypergraph>()
            .WithMany()
            .HasForeignKey(m => m.HypergraphId);

        builder.Property(h => h.FeeAddress)
            .HasMaxLength(DatabaseConstants.ColumnTypeLengths.WalletText)
            .IsRequired(false)
            .HasConversion(
                id => id!.Value,
                value => new WalletAddress(value)
            );

        builder.Property(h => h.StakingAddress)
            .HasMaxLength(DatabaseConstants.ColumnTypeLengths.WalletText)
            .IsRequired(false)
            .HasConversion(
                id => id!.Value,
                value => new WalletAddress(value)
            );

        builder.Property(m => m.Name)
            .HasMaxLength(DatabaseConstants.ColumnTypeLengths.NormalText)
            .IsRequired();

        builder.Property(m => m.Symbol)
            .HasMaxLength(DatabaseConstants.ColumnTypeLengths.TinyText)
            .IsRequired();

        builder.Property(m => m.CompanyName)
            .HasMaxLength(DatabaseConstants.ColumnTypeLengths.NormalText)
            .IsRequired();

        builder.Property(m => m.Description)
            .HasMaxLength(DatabaseConstants.ColumnTypeLengths.ExtraLongText)
            .IsRequired(false);

        builder.Property(m => m.Website)
            .HasMaxLength(DatabaseConstants.ColumnTypeLengths.LongText)
            .IsRequired(false);

        builder.Property(m => m.DataSyncEnabled)
            .HasDefaultValue(false);

        builder.OwnsMany<MetagraphEndpoint>(m => m.MetagraphEndpoint, endpointBuilder =>
        {
            endpointBuilder.ToJson();

            endpointBuilder.Property(me => me.ApiBaseAddress)
                .HasMaxLength(DatabaseConstants.ColumnTypeLengths.MediumText)
                .IsRequired();

            endpointBuilder.Property(v => v.MetagraphType)
                .HasMaxLength(DatabaseConstants.ColumnTypeLengths.ShortText)
                .HasConversion<string>()
                .IsRequired();
        });
    }
}
