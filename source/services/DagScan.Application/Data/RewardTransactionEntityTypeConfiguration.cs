using DagScan.Application.Domain;
using DagScan.Application.Domain.ValueObjects;
using DagScan.Core.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DagScan.Application.Data;

public class RewardTransactionEntityTypeConfiguration: IEntityTypeConfiguration<RewardTransaction>
{
    public void Configure(EntityTypeBuilder<RewardTransaction> builder)
    {
        builder.ToTable("RewardTransactions");

        builder.Property(x => x.Id)
            .ValueGeneratedNever()
            .HasConversion(
                id => id.Value,
                value => new RewardTransactionId(value)
            );

        builder.Property(x => x.RewardTransactionConfigId)
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

        builder.Property(x => x.WalletAddress)
            .HasMaxLength(DatabaseConstants.ColumnTypeLengths.WalletText)
            .IsRequired()
            .HasConversion(
                id => id.Value,
                value => new WalletAddress(value)
            );

        builder.Property(x => x.MetagraphAddress)
            .HasMaxLength(DatabaseConstants.ColumnTypeLengths.NormalText)
            .IsRequired(false)
            .HasConversion(
                id => id!.Value,
                value => new MetagraphAddress(value)
            );

        builder.Property(x => x.RewardCategory)
            .HasMaxLength(DatabaseConstants.ColumnTypeLengths.MediumText)
            .IsRequired();

        builder.Property(x => x.TransactionHash)
            .HasMaxLength(DatabaseConstants.ColumnTypeLengths.LongText)
            .IsRequired();
    }
}
