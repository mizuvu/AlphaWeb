using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public abstract class ApiControllerBase : ControllerBase
{
    /// <summary>
    /// Abstract BaseApi Controller Class
    /// </summary>
    private ISender _mediator = null!;
    protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<ISender>();
}


public abstract class ApiControllerBase<T> : ApiControllerBase
{
    /// <summary>
    /// Abstract BaseApi Controller Class
    /// </summary>
    private ILogger<T> _logger = null!;
    protected ILogger<T> Logger => _logger ??= HttpContext.RequestServices.GetRequiredService<ILogger<T>>();
}
