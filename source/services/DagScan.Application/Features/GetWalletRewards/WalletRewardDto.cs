using DagScan.Application.Domain.ValueObjects;

namespace DagScan.Application.Features.GetWalletRewards;

public record WalletRewardDto(
    string WalletAddress,
    DateTime TransactionDate,
    string? MetagraphAddress,
    long Amount,
    string RewardCategory,
    string? TransactionHash,
    long? Ordinal,
    string CurrencySymbol);
