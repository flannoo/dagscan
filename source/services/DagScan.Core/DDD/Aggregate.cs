using System.Collections.Concurrent;
using System.Collections.Immutable;
using DagScan.Core.CQRS;

namespace DagScan.Core.DDD;

public abstract class Aggregate<TId> : Entity<TId>, IAggregate<TId>
{
    [NonSerialized] private readonly ConcurrentQueue<IDomainEvent> _domainEvents = new();

    public int ConcurrencyVersion { get; private set; } = default!;

    public IReadOnlyList<IDomainEvent> GetDomainEvents()
    {
        return _domainEvents.ToImmutableList();
    }

    public bool HasDomainEvents()
    {
        return !_domainEvents.IsEmpty;
    }

    protected void AddDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents.Enqueue(domainEvent);
    }
}
