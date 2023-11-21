using MediatR;

namespace DelayMessageBus1.API.Application;

public interface ICommand : IRequest
{
    
}

public interface ICommand<out TResponse> : IRequest<TResponse>
{
    
}

public interface ICommandHandler<TRequest, TResponse> : IRequestHandler<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    
}

public interface ICommandHandler<TRequest> : IRequestHandler<TRequest> where TRequest : IRequest
{
    
}