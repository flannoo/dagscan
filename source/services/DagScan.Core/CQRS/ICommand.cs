using MediatR;

namespace DagScan.Core.CQRS;

public interface ICommand<out TResult> : IRequest<TResult>
{
}
