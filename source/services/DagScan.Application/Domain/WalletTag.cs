using DagScan.Application.Domain.ValueObjects;
using DagScan.Core.DDD;

namespace DagScan.Application.Domain;

public sealed class WalletTag : Entity<int>
{
    public WalletAddress WalletAddress { get; init; } = default!;
    public string Tag { get; init; } = default!;
}
