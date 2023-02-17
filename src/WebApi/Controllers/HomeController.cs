using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[AllowAnonymous]
[Route("/")]
public class HomeController : Controller
{
    [HttpGet]
    public IActionResult GetAsync()
    {
        return LocalRedirect("/swagger");
    }
}
