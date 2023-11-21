namespace DelayMessageBus2.API.Application
{
    public class CreateObjectService
    {
        private readonly ILogger<CreateObjectService> _logger;
        private readonly IMessageBus _messageBus;

        public CreateObjectService(ILogger<CreateObjectService> logger
            , IMessageBus messageBus)
        {
            _logger = logger;
            _messageBus = messageBus;
        }

        public Task Execute()
        {
            _logger.LogInformation("Object Created");

            _messageBus.SendMessage();
            
            return Task.CompletedTask;
        }
    }
}