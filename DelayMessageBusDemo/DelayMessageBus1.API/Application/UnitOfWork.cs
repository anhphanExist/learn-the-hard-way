namespace DelayMessageBus1.API.Application;

public class UnitOfWork : IUnitOfWork
{
    private readonly ILogger<UnitOfWork> _logger;
    
    public UnitOfWork(ILogger<UnitOfWork> logger)
    {
        _logger = logger;
    }

    public Task BeginTransaction(CancellationToken cancellationToken)
    {
        CurrentTransactionId = Guid.NewGuid();
        _logger.LogInformation("Begin transaction, id = {CurrentTransactionId}", CurrentTransactionId);
        return Task.CompletedTask;
    }

    public Task RollbackTransaction(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Rollback transaction, id = {CurrentTransactionId}", CurrentTransactionId);
        CurrentTransactionId = null;
        return Task.CompletedTask;
    }

    public Task CommitTransaction(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Commit transaction, id = {CurrentTransactionId}", CurrentTransactionId);
        CurrentTransactionId = null;
        return Task.CompletedTask;
    }

    public bool IsAlreadyHaveTransaction => CurrentTransactionId != null;
    public Guid? CurrentTransactionId { get; private set; }
}