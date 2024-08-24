using Ardalis.GuardClauses;

namespace DagScan.Core.DDD;

public record EntityId<T> : Identity<T>
{
    public static implicit operator T(EntityId<T> id) => Guard.Against.Null(id.Value, nameof(id.Value));

    public static EntityId<T> CreateEntityId(T id) => new() { Value = id };
}

public record EntityId : EntityId<Guid>
{
    public static implicit operator Guid(EntityId id) => Guard.Against.Null(id.Value, nameof(id.Value));

    public static new EntityId CreateEntityId(Guid id) => new() { Value = id };
}
