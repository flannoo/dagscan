using DagScan.Application.Domain;
using DagScan.Core.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DagScan.Application.Data;

public sealed class NodeOperatorEntityTypeConfiguration : IEntityTypeConfiguration<NodeOperator>
{
    public void Configure(EntityTypeBuilder<NodeOperator> builder)
    {
        builder.ToTable("NodeOperators");

        builder.Property(n => n.Id)
            .ValueGeneratedNever()
            .HasConversion(
                id => id.Value,
                value => new NodeOperatorId(value)
            );

        builder.HasKey(n => n.Id);

        builder.Property(n => n.Name)
            .HasMaxLength(DatabaseConstants.ColumnTypeLengths.MediumText)
            .IsRequired();

        builder.Property(n => n.Description)
            .HasMaxLength(DatabaseConstants.ColumnTypeLengths.ExtraLongText)
            .IsRequired(false);

        builder.Property(n => n.Website)
            .HasMaxLength(DatabaseConstants.ColumnTypeLengths.MediumText)
            .IsRequired(false);
    }
}
