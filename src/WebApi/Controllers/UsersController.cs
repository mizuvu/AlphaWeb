using Application.Common.Interfaces;
using Application.Contracts.Authorization;
using Application.Contracts.Identity;
using Application.Identity;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[Authorize(Policy = Permissions.Users.View)]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly ICurrentUser _currentUser;

    public UsersController(
        IUserService userService,
        ICurrentUser currentUser)
    {
        _userService = userService;
        _currentUser = currentUser;
    }

    [HttpGet]
    public async Task<IActionResult> GetAsync(CancellationToken cancellationToken)
    {
        var canViewAll = _currentUser.IsMasterUser;
        return Ok(await _userService.GetAllAsync(canViewAll, cancellationToken));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetAsync([FromRoute] string id)
    {
        return Ok(await _userService.GetByIdAsync(id));
    }

    [HttpPost]
    [Authorize(Policy = Permissions.Users.Create)]
    public async Task<IActionResult> PostAsync([FromBody] UserDto request)
    {
        return Ok(await _userService.CreateAsync(request));
    }

    [HttpPut]
    [Authorize(Policy = Permissions.Users.Update)]
    public async Task<IActionResult> PutAsync([FromBody] UserDto request)
    {
        return Ok(await _userService.UpdateAsync(request));
    }

    [HttpDelete("{id}")]
    [Authorize(Policy = Permissions.Users.Delete)]
    public async Task<IActionResult> DeleteAsync([FromQuery] string id)
    {
        return Ok(await _userService.DeleteAsync(id));
    }

    [HttpPut("{id}/force-password")]
    [Authorize(Policy = Permissions.Users.Update)]
    public async Task<IActionResult> ForcePasswordAsync([FromRoute] string id, [FromBody] string password)
    {
        return Ok(await _userService.ForcePasswordAsync(id, password));
    }
}