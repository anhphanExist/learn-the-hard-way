using System;
using System.Threading;
using System.Threading.Tasks;

namespace DelayMessageBus1.API.Application;

public interface IUnitOfWork
{
    Task BeginTransaction(CancellationToken cancellationToken);

    Task RollbackTransaction(CancellationToken cancellationToken);

    Task CommitTransaction(CancellationToken cancellationToken);
    
    bool IsAlreadyHaveTransaction { get; }
    
    Guid? CurrentTransactionId { get; }
}