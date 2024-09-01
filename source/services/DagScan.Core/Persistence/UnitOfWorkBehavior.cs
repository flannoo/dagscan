using DagScan.Core.CQRS;
using MediatR;

namespace DagScan.Core.Persistence;

public sealed class UnitOfWorkBehavior<TCommand, TResponse>(IEfUnitOfWork unitOfWork) :
    IPipelineBehavior<TCommand, TResponse>
    where TCommand : ICommand<TResponse>
{
    public async Task<TResponse> Handle(TCommand request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        await using var transaction = await unitOfWork.DbContext.Database.BeginTransactionAsync(cancellationToken);

        var response = await next();

        await unitOfWork.CommitAsync(cancellationToken);
        await transaction.CommitAsync(cancellationToken);

        unitOfWork.DbContext.ChangeTracker.Clear();

        return response;
    }
}
