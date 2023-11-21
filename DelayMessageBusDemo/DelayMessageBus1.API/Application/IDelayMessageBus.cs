namespace DelayMessageBus1.API.Application;

public interface IDelayMessageBus : IMessageBus
{
    Task SendMessageReal();
}