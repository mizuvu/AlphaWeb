using Application.Identity;
using Infrastructure.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers;

[Authorize]
[AutoValidateAntiforgeryToken]
public class AccountController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IActiveDirectoryService _adService;

    public AccountController(UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        IActiveDirectoryService adService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _adService = adService;
    }

    [AllowAnonymous]
    public async Task<IActionResult> LoginAsync(LoginRequest request, string? returnUrl = null)
    {
        returnUrl ??= Url.Content("~/");

        if (Request.Method == "GET")
        {
            return View(request);
        }

        if (!ModelState.IsValid)
        {
            return View(request);
        }

        var user = await _userManager.FindByNameAsync(request.UserName);

        if (user == null)
        {
            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return View();
        }

        if (user.IsBlocked)
        {
            ModelState.AddModelError(string.Empty, "User is blocked.");
            return View();
        }

        bool isLoginSucceeded;

        if (user.UseDomainPassword)
        {
            var loginByDomain = await _adService.CheckPasswordSignInAsync(request.UserName, request.Password);

            if (loginByDomain)
            {
                await _signInManager.SignInAsync(user, isPersistent: false);
            }

            isLoginSucceeded = loginByDomain;
        }
        else
        {
            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, set lockoutOnFailure: true
            var loginByLocal = await _signInManager.PasswordSignInAsync(request.UserName, request.Password, request.RememberMe, lockoutOnFailure: false);

            isLoginSucceeded = loginByLocal.Succeeded;
        }

        if (isLoginSucceeded)
        {
            return LocalRedirect(returnUrl);
        }

        ModelState.AddModelError(string.Empty, "Invalid login attempt.");

        return View();
    }

    // AccountController/Logout
    public async Task<IActionResult> Logout(string? returnUrl)
    {
        await _signInManager.SignOutAsync();

        if (returnUrl != null)
        {
            return LocalRedirect(returnUrl);
        }
        else
        {
            return RedirectToAction("Login");
        }
    }

    public IActionResult AccessDenied()
    {
        return View();
    }
}
