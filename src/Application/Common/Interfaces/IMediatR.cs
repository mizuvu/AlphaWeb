using MediatR;

namespace Application.Common.Interfaces;

public interface IQuery<TResponse> : IRequest<TResponse>
{
}

public interface IQueryHandler<in TRequest, TResponse> : IRequestHandler<TRequest, TResponse>
    where TRequest : IQuery<TResponse>
{
}

public interface ICommand<TResponse> : IRequest<TResponse>
{
}

public interface ICommand : ICommand<Result>
{
}

public interface ICommandHandler<in TRequest, TResponse> : IRequestHandler<TRequest, TResponse>
    where TRequest : ICommand<TResponse>
{
}

public interface ICommandHandler<in TRequest> : ICommandHandler<TRequest, Result>
    where TRequest : ICommand<Result>
{
}