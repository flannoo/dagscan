namespace DagScan.Core.DDD;

public interface IAggregate<out TId> : IEntity<TId>, IHaveAggregate
{
}

public interface IHaveAggregate : IHaveDomainEvents
{
}
