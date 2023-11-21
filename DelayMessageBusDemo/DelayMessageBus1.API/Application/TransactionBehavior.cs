using System.Text.Json;
using MediatR;

namespace DelayMessageBus1.API.Application;

public class TransactionBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
{
    private readonly ILogger<TransactionBehavior<TRequest, TResponse>> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDelayMessageBus _delayMessageBus;

    public TransactionBehavior(ILogger<TransactionBehavior<TRequest, TResponse>> logger
        , IUnitOfWork unitOfWork
        , IDelayMessageBus delayMessageBus)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _delayMessageBus = delayMessageBus;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var response = default(TResponse);
        var typeName = request.GetGenericTypeName();

        try
        {
            var requestIsQueryType = request.GetType().GetInterface(typeof(IQuery<>).Name) is not null;
            if (requestIsQueryType)
            {
                return await next();
            }
            
            if (_unitOfWork.IsAlreadyHaveTransaction)
            {
                return await next();
            }

            await _unitOfWork.BeginTransaction(cancellationToken);
            _logger.LogInformation("----- Begin transaction {TransactionId} for {CommandName} ({request})", _unitOfWork.CurrentTransactionId, typeName, JsonSerializer.Serialize(request));

            response = await next();

            _logger.LogInformation("----- Commit transaction {TransactionId} for {CommandName}", _unitOfWork.CurrentTransactionId, typeName);

            try
            {
                await _unitOfWork.CommitTransaction(cancellationToken);
                await _delayMessageBus.SendMessageReal();
            }
            catch
            {
                await _unitOfWork.RollbackTransaction(cancellationToken);
                throw;
            }   
            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "ERROR Handling transaction for {CommandName} ({@Command})", typeName, request);

            throw;
        }
    }
}