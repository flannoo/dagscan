using DagScan.Application.Domain;
using DagScan.Application.Domain.ValueObjects;
using DagScan.Core.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DagScan.Application.Data;

public sealed class RewardTransactionConfigEntityTypeConfiguration : IEntityTypeConfiguration<RewardTransactionConfig>
{
    public void Configure(EntityTypeBuilder<RewardTransactionConfig> builder)
    {
        builder.ToTable("RewardTransactionConfigs");

        builder.Property(x => x.Id)
            .ValueGeneratedNever()
            .HasConversion(
                id => id.Value,
                value => new RewardTransactionConfigId(value)
            );

        builder.Property(x => x.MetagraphId)
            .ValueGeneratedNever()
            .HasConversion(
                id => id.Value,
                value => new MetagraphId(value)
            );

        builder.HasOne<Metagraph>()
            .WithMany()
            .HasForeignKey(x => x.MetagraphId);

        builder.Property(x => x.MetagraphAddress)
            .ValueGeneratedNever()
            .HasConversion(
                id => id!.Value,
                value => new MetagraphAddress(value)
            );

        builder.Property(x => x.RewardCategory)
            .HasMaxLength(DatabaseConstants.ColumnTypeLengths.MediumText)
            .IsRequired();

        builder.Property(x => x.FromWalletAddress)
            .ValueGeneratedNever()
            .HasConversion(
                id => id!.Value,
                value => new WalletAddress(value)
            );

        builder.Property(x => x.ToWalletAddress)
            .ValueGeneratedNever()
            .HasConversion(
                id => id!.Value,
                value => new WalletAddress(value)
            );

        builder.Property(x => x.LastProcessedHash)
            .HasMaxLength(DatabaseConstants.ColumnTypeLengths.LongText);
    }
}
