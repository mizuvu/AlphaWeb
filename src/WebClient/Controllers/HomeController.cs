using Application.Contracts.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebClient.Models;

namespace WebClient.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly TokenClient _tokenClient;

    public HomeController(ILogger<HomeController> logger,
        TokenClient tokenClient)
    {
        _logger = logger;
        _tokenClient = tokenClient;
    }

    public async Task<IActionResult> Index()
    {
        var request = new TokenRequest("super", "123");
        var token = await _tokenClient.GetTokenAsync(request);
        ViewBag.Token = token.AccessToken;
        ViewBag.RefreshToken = token.RefreshToken;

        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}