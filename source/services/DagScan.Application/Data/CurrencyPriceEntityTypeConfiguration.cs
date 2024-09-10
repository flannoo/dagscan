using DagScan.Application.Domain;
using DagScan.Application.Domain.ValueObjects;
using DagScan.Core.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DagScan.Application.Data;

public sealed class CurrencyPriceEntityTypeConfiguration : IEntityTypeConfiguration<CurrencyPrice>
{
    public void Configure(EntityTypeBuilder<CurrencyPrice> builder)
    {
        builder.ToTable("CurrencyPrices");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .UseIdentityColumn();

        builder.Property(x => x.MetagraphAddress)
            .HasMaxLength(DatabaseConstants.ColumnTypeLengths.NormalText)
            .IsRequired(false)
            .HasConversion(
                id => id!.Value,
                value => new MetagraphAddress(value)
            );

        builder.Property(x => x.Price)
            .HasColumnType("decimal(30, 18)")
            .HasPrecision(30, 18);
    }
}
