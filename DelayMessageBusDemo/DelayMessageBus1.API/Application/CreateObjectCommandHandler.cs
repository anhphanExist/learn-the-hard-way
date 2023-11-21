namespace DelayMessageBus1.API.Application
{
    public class CreateObjectCommandHandler : ICommandHandler<CreateObjectCommand>
    {
        private readonly ILogger<CreateObjectCommandHandler> _logger;
        private readonly IMessageBus _messageBus;

        public CreateObjectCommandHandler(ILogger<CreateObjectCommandHandler> logger, IMessageBus messageBus)
        {
            _logger = logger;
            _messageBus = messageBus;
        }

        public Task Handle(CreateObjectCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Object Created");

            _messageBus.SendMessage();
            
            return Task.CompletedTask;
        }
    }
}