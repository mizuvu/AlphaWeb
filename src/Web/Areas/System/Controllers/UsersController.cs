using Application.Common.Interfaces;
using Application.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Web.Areas.System.Controllers;

[AutoValidateAntiforgeryToken]
[MustHavePermission(Permissions.Users.View)]
public partial class UsersController : BaseController
{
    private readonly IUserService _userService;
    private readonly IActiveDirectoryService _adService;
    private readonly ICurrentUser _currentUser;

    public UsersController(
        IUserService userService,
        IActiveDirectoryService adService,
        ICurrentUser currentUser)
    {
        _userService = userService;
        _adService = adService;
        _currentUser = currentUser;
    }

    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var isMasterUser = _currentUser.IsMasterUser;
        var data = await _userService.GetAllAsync(isMasterUser, cancellationToken);
        return View(data);
    }
}
