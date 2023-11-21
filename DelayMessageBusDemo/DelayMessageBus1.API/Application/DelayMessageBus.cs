using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace DelayMessageBus1.API.Application;

public class DelayMessageBus : IDelayMessageBus
{
    private readonly ILogger<DelayMessageBus> _logger;

    public DelayMessageBus(ILogger<DelayMessageBus> logger)
    {
        _logger = logger;
    }

    public Task SendMessage()
    {
        _logger.LogInformation("Save message to temporary queue");
        return Task.CompletedTask;
    }

    public Task SendMessageReal()
    {
        _logger.LogInformation("Send message in temporary queue to destination");
        return Task.CompletedTask;
    }
}