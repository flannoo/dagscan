namespace DagScan.Core.DDD;

public abstract class Aggregate<TId> : Entity<TId>, IAggregate<TId>
{

}
