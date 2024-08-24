using DagScan.Core.CQRS;
using DagScan.Core.DDD;
using DagScan.Core.Persistence;
using MediatR;

namespace DagScan.Core.Messaging;

public sealed class PublishDomainEventBehavior<TCommand, TResponse>(
    IEfUnitOfWork unitOfWork,
    IMediator mediator) :
    IPipelineBehavior<TCommand, TResponse>
    where TCommand : ICommand<TResponse>
{
    public async Task<TResponse> Handle(TCommand request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var response = await next();

        var domainEvents = unitOfWork.DbContext.ChangeTracker.Entries()
            .Where(e => e.Entity is IHaveDomainEvents)
            .SelectMany(e => ((IHaveDomainEvents)e.Entity).GetDomainEvents())
            .ToList()
            .Where(domainEvent => domainEvent is INotification);

        foreach (var domainEvent in domainEvents)
        {
            await mediator.Publish(domainEvent, cancellationToken);
        }

        return response;
    }
}
