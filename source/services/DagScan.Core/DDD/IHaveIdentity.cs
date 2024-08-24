namespace DagScan.Core.DDD;

public interface IHaveIdentity<out TId>
{
    TId Id { get; }
}
