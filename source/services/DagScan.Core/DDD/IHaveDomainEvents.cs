using DagScan.Core.CQRS;

namespace DagScan.Core.DDD;

public interface IHaveDomainEvents
{
    public bool HasDomainEvents();

    IReadOnlyList<IDomainEvent> GetDomainEvents();
}
