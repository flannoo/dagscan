using Microsoft.EntityFrameworkCore;

namespace DagScan.Core.Persistence;

public interface IUnitOfWork
{
    Task CommitAsync(CancellationToken cancellationToken);
}

public interface IEfUnitOfWork : IUnitOfWork
{
    DbContext DbContext { get; }
}
