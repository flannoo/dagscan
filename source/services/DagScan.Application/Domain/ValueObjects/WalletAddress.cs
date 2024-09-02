using Ardalis.GuardClauses;
using DagScan.Core.DDD;
using DagScan.Core.Extensions;

namespace DagScan.Application.Domain.ValueObjects;

public sealed class WalletAddress : ValueObject
{
    public string Value { get; }
    
    public static WalletAddress CreateFromWalletId(string walletId)
    {
        Guard.Against.NullOrWhiteSpace(walletId, nameof(walletId));
        return new WalletAddress(walletId.ConvertNodeIdToWalletHash());
    }

    public WalletAddress(string value)
    {
        Guard.Against.NullOrWhiteSpace(value, nameof(value));

        Value = value;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
