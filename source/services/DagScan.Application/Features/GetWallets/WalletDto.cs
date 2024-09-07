using DagScan.Application.Domain.ValueObjects;

namespace DagScan.Application.Features.GetWallets;

public sealed record WalletDto(
    int Rank,
    string Address,
    string? Tag,
    long Balance,
    long UsdValue,
    double SupplyPercent);

public sealed record WalletInfo(WalletAddress WalletAddress, long Balance);
