using System.Threading.Tasks;

namespace DelayMessageBus2.API.Application
{
    public interface IDelayMessageBus : IMessageBus
    {
        Task SendMessageReal();
    }
}