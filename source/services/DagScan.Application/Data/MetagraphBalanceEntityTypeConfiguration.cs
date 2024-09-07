using DagScan.Application.Domain;
using DagScan.Application.Domain.ValueObjects;
using DagScan.Core.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DagScan.Application.Data;

public sealed class MetagraphBalanceEntityTypeConfiguration : IEntityTypeConfiguration<MetagraphBalance>
{
    public void Configure(EntityTypeBuilder<MetagraphBalance> builder)
    {
        builder.ToTable("MetagraphBalances");

        builder.Property(x => x.Id)
            .ValueGeneratedNever()
            .HasConversion(
                id => id.Value,
                value => new MetagraphBalanceId(value)
            );

        builder.Property(x => x.MetagraphId)
            .ValueGeneratedNever()
            .HasConversion(
                id => id.Value,
                value => new MetagraphId(value)
            );

        builder.HasOne<Metagraph>()
            .WithMany()
            .HasForeignKey(h => h.MetagraphId);

        builder.Property(x => x.MetagraphAddress)
            .HasMaxLength(DatabaseConstants.ColumnTypeLengths.NormalText)
            .IsRequired()
            .HasConversion(
                id => id.Value,
                value => new MetagraphAddress(value)
            );

        builder.HasKey(x => x.Id);

        builder.Property(x => x.WalletAddress)
            .HasMaxLength(DatabaseConstants.ColumnTypeLengths.WalletText)
            .IsRequired()
            .HasConversion(
                id => id.Value,
                value => new WalletAddress(value)
            );

        builder.HasIndex(x => x.MetagraphId)
            .IncludeProperties(x => new { x.Balance, x.MetagraphAddress, x.WalletAddress })
            .IsUnique(false)
            .IsCreatedOnline(true);
    }
}
