using System.Threading.Tasks;
using DelayMessageBus2.API.Application;
using DelayMessageBus2.API.InProcessJobs;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DelayMessageBus2.API.Controllers;

[ApiController]
[Route("[controller]")]
public class DelayMessageController : ControllerBase
{
    private readonly ILogger<DelayMessageController> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly CreateObjectService _createObjectService;
    private readonly IDelayMessageBus _delayMessageBus;
    private readonly ISender _sender;

    public DelayMessageController(ILogger<DelayMessageController> logger
        , IUnitOfWork unitOfWork
        , CreateObjectService createObjectService
        , IDelayMessageBus delayMessageBus
        , ISender sender)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _createObjectService = createObjectService;
        _delayMessageBus = delayMessageBus;
        _sender = sender;
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateObject()
    {
        if (!_unitOfWork.IsAlreadyHaveTransaction)
        {
            await _unitOfWork.BeginTransaction();
            _logger.LogInformation("----- Begin transaction {TransactionId} for {action}", _unitOfWork.CurrentTransactionId, nameof(CreateObject));

            await _createObjectService.Execute();

            _logger.LogInformation("----- Commit transaction {TransactionId} for {action}", _unitOfWork.CurrentTransactionId, nameof(CreateObject));

            try
            {
                await _unitOfWork.CommitTransaction();
                await _delayMessageBus.SendMessageReal();
            }
            catch
            {
                await _unitOfWork.RollbackTransaction();
                throw;
            }

            return Ok();
        }

        await _createObjectService.Execute();
        return Ok();
    }

    [HttpPost("inprocess")]
    public async Task<IActionResult> CreateObjectInProcessJob()
    {
        await _sender.Send(new CreateObjectCommand());
        return Ok();
    }
}