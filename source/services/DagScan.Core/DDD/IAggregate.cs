namespace DagScan.Core.DDD;

public interface IAggregate<out TId> : IEntity<TId> { }
