namespace DelayMessageBus1.API.Application
{
    public interface IMessageBus
    {
        Task SendMessage();
    }
}