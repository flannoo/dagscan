namespace DagScan.Core.CQRS;

public abstract record DomainEvent : IDomainEvent
{
    public dynamic AggregateId { get; protected set; } = null!;

    public virtual IDomainEvent WithAggregate(dynamic aggregateId)
    {
        AggregateId = aggregateId;
        return this;
    }
}
