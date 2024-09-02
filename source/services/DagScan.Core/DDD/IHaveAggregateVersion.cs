namespace DagScan.Core.DDD;

public interface IHaveAggregateVersion
{
    int ConcurrencyVersion { get; }
}
