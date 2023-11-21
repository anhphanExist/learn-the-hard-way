namespace DelayMessageBus2.API.Application;

public interface IUnitOfWork
{
    Task BeginTransaction(CancellationToken cancellationToken = default);

    Task RollbackTransaction(CancellationToken cancellationToken = default);

    Task CommitTransaction(CancellationToken cancellationToken = default);
    
    bool IsAlreadyHaveTransaction { get; }
    
    Guid? CurrentTransactionId { get; }
}