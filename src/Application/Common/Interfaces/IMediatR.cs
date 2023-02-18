using MediatR;

namespace Application.Common.Interfaces;

/// <summary>
/// Execute a query for read data from Database
/// </summary>
/// <typeparam name="TResponse"></typeparam>
public interface IQuery<TResponse> : IRequest<TResponse> { }

public interface IQueryHandler<in TRequest, TResponse> : IRequestHandler<TRequest, TResponse>
    where TRequest : IQuery<TResponse>
{ }


/// <summary>
/// Execute a command Insert, Update, Delete affect on Database with return value
/// </summary>
public interface ICommand<TResponse> : IRequest<TResponse> { }

public interface ICommandHandler<in TRequest, TResponse> : IRequestHandler<TRequest, TResponse>
    where TRequest : ICommand<TResponse>
{ }

public interface ICommand : ICommand<Result> { }

public interface ICommandHandler<in TRequest> : ICommandHandler<TRequest, Result>
    where TRequest : ICommand<Result>
{ }

/// <summary>
/// Execute a command with no return value
/// </summary>
public interface INonQuery : IRequest { }

public interface INonQueryHandler<in TRequest> : IRequestHandler<TRequest>
    where TRequest : INonQuery
{ }