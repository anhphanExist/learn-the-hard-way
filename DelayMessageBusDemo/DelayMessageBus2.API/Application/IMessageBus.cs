namespace DelayMessageBus2.API.Application
{
    public interface IMessageBus
    {
        Task SendMessage();
    }
}