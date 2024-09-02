namespace DagScan.Core.DDD;

public interface IEntity<out TId> : IHaveIdentity<TId>
{
}

public interface IEntity : IEntity<EntityId> { }
