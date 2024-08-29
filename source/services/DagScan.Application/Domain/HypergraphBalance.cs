using DagScan.Application.Domain.ValueObjects;
using DagScan.Core.DDD;

namespace DagScan.Application.Domain;

public sealed class HypergraphBalance : Entity<HypergraphBalanceId>
{
    public HypergraphId HypergraphId { get; private set; } = default!;
    public WalletAddress WalletAddress { get; private set; } = default!;
    public long Balance { get; private set; }

    public static HypergraphBalance Create(HypergraphId hypergraphId, WalletAddress walletAddress, long balance)
    {
        return new HypergraphBalance()
        {
            Id = new HypergraphBalanceId(Guid.NewGuid()),
            HypergraphId = hypergraphId,
            WalletAddress = walletAddress,
            Balance = balance
        };
    }
}
