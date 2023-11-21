namespace DelayMessageBus2.API.Application
{
    public interface IDelayMessageBus : IMessageBus
    {
        Task SendMessageReal();
    }
}