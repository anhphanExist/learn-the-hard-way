using DelayMessageBus1.API.Application;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DelayMessageBus1.API.Controllers;

[ApiController]
[Route("[controller]")]
public class DelayMessageController : ControllerBase
{
    private readonly ILogger<DelayMessageController> _logger;
    private readonly ISender _sender;

    public DelayMessageController(ILogger<DelayMessageController> logger, ISender sender)
    {
        _logger = logger;
        _sender = sender;
    }

    [HttpPost]
    public async Task<IActionResult> CreateObject()
    {
        await _sender.Send(new CreateObjectCommand());
        return Ok();
    }
}