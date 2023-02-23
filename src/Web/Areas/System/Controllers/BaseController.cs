using Microsoft.AspNetCore.Mvc;

namespace Web.Areas.System.Controllers;

[Area("System")]
public class BaseController : Controller
{
}

public class BaseController<T> : BaseController
{
    /// <summary>
    /// Abstract BaseApi Controller Class
    /// </summary>
    private ILogger<T> _logger = null!;
    protected ILogger<T> Logger => _logger ??= HttpContext.RequestServices.GetRequiredService<ILogger<T>>();
}