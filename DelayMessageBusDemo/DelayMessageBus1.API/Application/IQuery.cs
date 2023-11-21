using MediatR;

namespace DelayMessageBus1.API.Application
{
    public interface IQuery<out TResponse> : IRequest<TResponse>
    {
        
    }
}