namespace DagScan.Application.Features.GetWalletBalances;

public record MetagraphBalancesDto(string MetagraphAddress, string TokenSymbol, long Balance);

public record WalletBalancesDto(string Address, long Balance, string? Tag, List<MetagraphBalancesDto> MetagraphBalances);
