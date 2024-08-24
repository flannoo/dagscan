using DagScan.Core.Domain;
using DagScan.Core.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DagScan.Core.Data;

public sealed class HypergraphEntityTypeConfiguration : IEntityTypeConfiguration<Hypergraph>
{
    public void Configure(EntityTypeBuilder<Hypergraph> builder)
    {
        builder.ToTable("Hypergraphs");

        builder.Property(h => h.Id)
            .ValueGeneratedNever()
            .HasConversion(
                id => id.Value,
                value => new HypergraphId(value)
            );

        builder.HasKey(h => h.Id);

        builder.Property(h => h.ApiBaseAddress)
            .HasMaxLength(DatabaseConstants.ColumnTypeLengths.NormalText)
            .IsRequired();
    }
}
