using MediatR;

namespace DagScan.Core.CQRS;

public interface IQuery<out TResult> : IRequest<TResult>
{
}
