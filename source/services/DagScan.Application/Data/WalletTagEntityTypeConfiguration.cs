using DagScan.Application.Domain;
using DagScan.Application.Domain.ValueObjects;
using DagScan.Core.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DagScan.Application.Data;

public sealed class WalletTagEntityTypeConfiguration : IEntityTypeConfiguration<WalletTag>
{
    public void Configure(EntityTypeBuilder<WalletTag> builder)
    {
        builder.ToTable("WalletTags");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .UseIdentityColumn();

        builder.Property(x => x.Tag)
            .HasMaxLength(DatabaseConstants.ColumnTypeLengths.NormalText);

        builder.Property(x => x.WalletAddress)
            .HasMaxLength(DatabaseConstants.ColumnTypeLengths.WalletText)
            .HasConversion(
                id => id!.Value,
                value => new WalletAddress(value)
            );
    }
}
