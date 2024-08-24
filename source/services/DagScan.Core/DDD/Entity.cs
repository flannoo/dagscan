namespace DagScan.Core.DDD;

public abstract class Entity<TId> : IEntity<TId>
{
    public TId Id { get; protected init; } = default!;
}

public abstract class Entity<TIdentity, TId> : Entity<TIdentity>
    where TIdentity : Identity<TId>
{ }

public abstract class Entity : Entity<EntityId, Guid>, IEntity { }
