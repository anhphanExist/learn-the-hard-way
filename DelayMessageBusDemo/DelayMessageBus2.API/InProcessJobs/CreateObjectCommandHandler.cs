using System.Threading;
using System.Threading.Tasks;
using DelayMessageBus2.API.Application;
using Microsoft.Extensions.Logging;

namespace DelayMessageBus2.API.InProcessJobs
{
    public class CreateObjectCommandHandler : ICommandHandler<CreateObjectCommand>
    {
        private readonly ILogger<CreateObjectCommandHandler> _logger;
        private readonly IDelayMessageBus _delayMessageBus;
        private readonly IUnitOfWork _unitOfWork;
        private readonly CreateObjectService _createObjectService;

        public CreateObjectCommandHandler(ILogger<CreateObjectCommandHandler> logger
            , IDelayMessageBus delayMessageBus
            , IUnitOfWork unitOfWork
            , CreateObjectService createObjectService)
        {
            _logger = logger;
            _delayMessageBus = delayMessageBus;
            _unitOfWork = unitOfWork;
            _createObjectService = createObjectService;
        }

        public async Task Handle(CreateObjectCommand request, CancellationToken cancellationToken)
        {
            var typeName = request.GetGenericTypeName();
            
            if (!_unitOfWork.IsAlreadyHaveTransaction)
            {
                await _unitOfWork.BeginTransaction(cancellationToken);
                _logger.LogInformation("----- Begin transaction {TransactionId} for {CommandName} ({@Command})", _unitOfWork.CurrentTransactionId, typeName, request);

                await _createObjectService.Execute();

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
            }

            await _createObjectService.Execute();
        }
    }
}