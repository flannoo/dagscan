using DagScan.Application.Domain.ValueObjects;
using DagScan.Core.DDD;

namespace DagScan.Application.Domain;

public sealed class MetagraphBalance : Entity<MetagraphBalanceId>
{
    public MetagraphId MetagraphId { get; private set; } = default!;
    public MetagraphAddress MetagraphAddress { get; private set; } = default!;
    public WalletAddress WalletAddress { get; private set; } = default!;
    public long Balance { get; private set; }

    public static MetagraphBalance Create(MetagraphId metagraphId, MetagraphAddress metagraphAddress, WalletAddress walletAddress, long balance)
    {
        return new MetagraphBalance()
        {
            Id = new MetagraphBalanceId(Guid.NewGuid()),
            MetagraphId = metagraphId,
            MetagraphAddress = metagraphAddress,
            WalletAddress = walletAddress,
            Balance = balance
        };
    }
}
